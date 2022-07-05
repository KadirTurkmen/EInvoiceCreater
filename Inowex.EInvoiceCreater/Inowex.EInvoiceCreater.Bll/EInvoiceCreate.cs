﻿using Inowex.EInvoiceCreater.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using UblTr.Common;
using UblTr.MainDoc;

namespace Inowex.EInvoiceCreater
{
    public interface IEInvoiceCreate
    {
        string InvoiceToXml(Invoice invoice);
    }

    public class EInvoiceCreate : IEInvoiceCreate
    {
        /// <summary>
        /// Fatura objesini Xml'e dönüştürür.
        /// </summary>
        /// <param name="invoice"></param>
        /// <returns></returns>
        public string InvoiceToXml(Invoice invoice)
        {
            var invoiceType = GetInvoiceType(invoice);
            string invoiceAsXml = InvoiceTypeToXml(invoiceType);
            WriteToDiskAsXml(invoiceAsXml, invoiceType.ID.Value);
            
            return invoiceAsXml;
        }

        /// <summary>
        /// Fatura bilgisini getirir.
        /// </summary>
        /// <param name="invoice"></param>
        /// <returns></returns>
        public InvoiceType GetInvoiceType(Invoice invoice)
        {
            string _customization = "TR1.2";
            string _ublVersion = "2.1";

            ///Birim fiyat * miktar
            decimal lineExtensionAmount = Math.Round(invoice.InvoiceLine.Sum(s => s.UnitPrice * s.Quantity), 2);
            ///Fatura matrahı
            decimal taxExlusiveAmount = Math.Round(invoice.InvoiceLine.Sum(s => s.LineTotalAmount), 2);
            ///Fatura toplam tutarı
            decimal taxInclusiveAmount = Math.Round(invoice.InvoiceLine.Sum(s => s.KDVAmount + s.OTVAmount + s.OIVAmount + s.UnitPrice * s.Quantity), 2);
            ///İskonto tutarı
            decimal allowanceTotalAmount = Math.Round(invoice.InvoiceLine.Sum(s => s.DiscountPrice), 2);
            ///Fatura toplam vergi tutarı
            decimal taxTotalAmount = Math.Round(invoice.InvoiceLine.Sum(s => s.KDVAmount + s.OTVAmount + s.OIVAmount), 2);
            ///Ödenecek tutar
            decimal payableAmount = lineExtensionAmount + allowanceTotalAmount + taxTotalAmount;

            var invoiceType = new UblTr.MainDoc.InvoiceType()
            {
                ///Ubl versiyonu kullanılan en son versiyon
                UBLVersionID = new UBLVersionIDType { Value = _ublVersion },
                ///Mevcut dökümanda belirtildiği gibi TR2.1
                CustomizationID = new CustomizationIDType { Value = _customization },
                ///Kullanılan fatura türü senaryosudur belirtilenler Ticari,Temel,EArşiv vb.
                ProfileID = new ProfileIDType { Value = invoice.ScenarioType.ToString() },
                ///Fatura numarasıdır GIB Zorunlu sonra gelen değer Yıl ve sonrasıda fatura sıra numarası. Örnek Format GIB2020000000001
                ID = new IDType { Value = invoice.No },
                ///Faturanın asıl veya suret olduğu bilgisidir. Asıl false, suret true.
                CopyIndicator = new CopyIndicatorType { Value = invoice.CopyIndicator },
                ///Guid.
                UUID = new UUIDType { Value = invoice.Guid.ToString() },
                ///Fatura oluşturulma tarihi.
                IssueDate = new IssueDateType { Value = invoice.Date },
                ///Fatura oluşturulma saat bilgisidir.
                IssueTime = new IssueTimeType { Value = invoice.Date },
                ///Fatura tip bilgisidir. ALIS veya SATIS
                InvoiceTypeCode = new InvoiceTypeCodeType { Value = invoice.InvioceType.ToString() },
                ///Fatura not veya açıklama bilgisidir.
                Note = GetInvoiceNote(invoice),
                ///Fatura döviz para birimi.
                DocumentCurrencyCode = new DocumentCurrencyCodeType { Value = invoice.CurrencyIso4217Code },
                ///Fatura kalem sayısıdır.
                LineCountNumeric = new LineCountNumericType { Value = invoice.InvoiceLine.Count() },
                ///Fatura dönem bilgisidir. 
                //InvoicePeriod= new PeriodType { },
                ///İlave döküman bilgisidir.
                //AdditionalDocumentReference = get_AdditionalDocumentReference(),
                ///Mali Mühür/İmza
                Signature = GetSignatureType(invoice),
                ///Tedarikçi bilgileri
                AccountingSupplierParty = GetSupplierParty(invoice),
                ///Müşteri bilgileri
                AccountingCustomerParty = GetCustomerParty(invoice),
                ///Vergi Tutarları
                TaxTotal = GetTaxTotal(invoice.InvoiceLine, invoice.CurrencyIso4217Code),
                ///Tevkifat Tutarları
                WithholdingTaxTotal = GetWithholdingTaxTotal(invoice.InvoiceLine, invoice.CurrencyIso4217Code),
                ///Toplam Tutar bilgileri
                LegalMonetaryTotal = new MonetaryTotalType
                {
                    LineExtensionAmount = new LineExtensionAmountType { Value = lineExtensionAmount, currencyID = invoice.CurrencyIso4217Code },///birim fiyat * miktar
                    TaxExclusiveAmount = new TaxExclusiveAmountType { Value = taxExlusiveAmount, currencyID = invoice.CurrencyIso4217Code },///Matrah
                    TaxInclusiveAmount = new TaxInclusiveAmountType { Value = taxInclusiveAmount, currencyID = invoice.CurrencyIso4217Code },///Toplam Tutar
                    AllowanceTotalAmount = new AllowanceTotalAmountType { Value = allowanceTotalAmount, currencyID = invoice.CurrencyIso4217Code },///İskonto tutar
                    PayableAmount = new PayableAmountType { Value = payableAmount, currencyID = invoice.CurrencyIso4217Code }///Ödenecek Tutar
                },
                ///Fatura Kalemleri bilgisidir.
                InvoiceLine = GetInvoiceLine(invoice.InvoiceLine, invoice.CurrencyIso4217Code),
            };

            return invoiceType;
        }

        /// <summary>
        /// Firma bilgilerini getirir.
        /// </summary>
        /// <returns></returns>
        public PartyType GetPartyType(CompanyInformation companyInformation)
        {
            ///Müşteri kimlik bilgileridir.
            List<PartyIdentificationType> partyIdentificationTypes = new List<PartyIdentificationType>();
            partyIdentificationTypes.Add(new PartyIdentificationType { ID = new IDType { schemeID = SchemeIds.VKN.ToString(), Value = companyInformation.TaxNumber } });
            if (companyInformation.MersisNo != null)
                partyIdentificationTypes.Add(new PartyIdentificationType { ID = new IDType { schemeID = SchemeIds.MERSISNO.ToString(), Value = companyInformation.MersisNo } });

            ///Firma adı bilgisidir.
            PartyNameType partyNameType = new PartyNameType { Name = new NameType1 { Value = companyInformation.Name } };

            ///Firma Daire numarası bilgisidir.
            RoomType roomType = new RoomType { Value = companyInformation.Room };

            ///Firma Adres Blok adı bilgisidir.
            BlockNameType blockNameType = new BlockNameType { Value = companyInformation.BlockName };

            ///Firma Bina adı bilgisidir.
            BuildingNameType buildingNameType = new BuildingNameType { Value = companyInformation.BuildingName };

            ///Firma Bina numarası bilgisidir.
            BuildingNumberType buildingNumberType = new BuildingNumberType { Value = companyInformation.BuildingNumber };

            ///Firma Adres ilçe bilgisidir.
            CitySubdivisionNameType citySubdivisionNameType = new CitySubdivisionNameType { Value = companyInformation.CitySubdivision };

            ///Firma adres il bilgisidir.
            CityNameType cityNameType = new CityNameType { Value = companyInformation.City };

            ///Firma adres ülke bilgisidir.
            CountryType countryType = new CountryType { Name = new NameType1 { Value = companyInformation.Country }, };

            ///Firma posta adresi bilgisidir.
            PostalZoneType postalZoneType = new PostalZoneType { Value = companyInformation.PostalZone };

            ///Firma mail adresi ve telefon bilgisidir.
            ContactType contactType = new ContactType { ElectronicMail = new ElectronicMailType { Value = companyInformation.Mail }, Telephone = new TelephoneType { Value = companyInformation.Phone } };

            ///Firma Vergi dairesi bilgisidir.
            PartyTaxSchemeType partyTaxSchemeType = new PartyTaxSchemeType { TaxScheme = new TaxSchemeType { Name = new NameType1 { Value = companyInformation.TaxOffice } } };

            ///Firma web site adresi bilgisidir..
            WebsiteURIType websiteURIType = new WebsiteURIType();
            if (companyInformation.WebSiteUri != null)
                websiteURIType = new WebsiteURIType { Value = companyInformation.WebSiteUri };

            return new PartyType
            {
                PartyIdentification = partyIdentificationTypes.ToArray(),
                PartyName = partyNameType,
                PostalAddress = new AddressType
                {
                    Room = roomType,
                    BlockName = blockNameType,
                    BuildingName = buildingNameType,
                    BuildingNumber = buildingNumberType,
                    CitySubdivisionName = citySubdivisionNameType,
                    CityName = cityNameType,
                    Country = countryType,
                    PostalZone = postalZoneType
                },
                WebsiteURI = websiteURIType,
                Contact = contactType,
                PartyTaxScheme = partyTaxSchemeType,
            };
        }

        /// <summary>
        /// Müşteri bilgilerini getirir.
        /// </summary>
        /// <returns></returns>
        public CustomerPartyType GetCustomerParty(Invoice invoice)
        {
            return new CustomerPartyType() { Party = GetPartyType(invoice.CustomerInfo) };
        }

        /// <summary>
        /// Tedarikçi bilgilerini betirir.
        /// </summary>
        /// <returns></returns>
        public SupplierPartyType GetSupplierParty(Invoice invoice)
        {
            return new SupplierPartyType() { Party = GetPartyType(invoice.SupplierInfo) };
        }

        /// <summary>
        /// İrsaliye, sipariş, kontrat, alındı ve diğer fatura belgeleri dışında
        /// faturaya eklenmek istenen diğer belgeler için bu eleman
        /// kullanılabilecektir.Örneğin belge para birimi dışında ayrıca
        /// fatura düzenlenmek isteniyorsa bu fatura düzenlenip
        /// AdditionalDocumentReference elemanının
        /// UBL-TR Fatura Aralık 2017
        /// Versiyon : 1.0 22/37
        /// EmbeddedDocumentBinaryObject elemanına eklenebilir.
        /// </summary>
        /// <returns></returns>
        public DocumentReferenceType[] get_AdditionalDocumentReference()
        {
            List<DocumentReferenceType> documentReferenceType = new List<DocumentReferenceType>();
            documentReferenceType.Add(new DocumentReferenceType
            {
                ID = new IDType { Value = Guid.NewGuid().ToString() },
                IssueDate = new IssueDateType { Value = DateTime.Now },
                DocumentType = new DocumentTypeType { Value = "XSLT" },
                Attachment = new AttachmentType
                {
                    EmbeddedDocumentBinaryObject = new EmbeddedDocumentBinaryObjectType
                    {
                        characterSetCode = "UTF-8",
                        encodingCode = "Base64",
                        filename = "EArchiveInvoice.xslt",
                        mimeCode = "application/xml",
                        Value = Encoding.UTF8.GetBytes(new StreamReader(new FileStream(AppDomain.CurrentDomain.BaseDirectory + "\\" + "general.xslt", FileMode.Open, FileAccess.Read), Encoding.UTF8).ReadToEnd()),
                    }
                }
            });
            documentReferenceType.Add(new DocumentReferenceType
            {
                ID = new IDType { Value = System.Guid.NewGuid().ToString() },
                IssueDate = new IssueDateType { Value = DateTime.Now },
                DocumentTypeCode = new DocumentTypeCodeType { Value = "SendingType" },
                DocumentType = new DocumentTypeType { Value = "ELEKTRONİK" }
            });

            return documentReferenceType.ToArray();
        }

        /// <summary>
        /// İskonto tutar bilgilerini getirir.
        /// </summary>
        /// <param name="faturaKalemDto"></param>
        /// <returns></returns>
        public AllowanceChargeType[] GetAllowanceChargeDiscount(InvoiceLine invoiceLine)
        {
            List<AllowanceChargeType> allowanceChargeTypes = new List<AllowanceChargeType>();

            if (invoiceLine.DiscountPrice != 0)
            {
                allowanceChargeTypes.Add(new AllowanceChargeType
                {
                    ///True Arttırım, False İskonto
                    ChargeIndicator = new ChargeIndicatorType { Value = invoiceLine.DiscountPrice > 0 ? true : false },
                    ///İskonto Oranı
                    MultiplierFactorNumeric = new MultiplierFactorNumericType { Value = invoiceLine.DiscountPercent / 100 },
                    ///İskonto Tutar
                    Amount = new AmountType2 { Value = invoiceLine.DiscountPrice, currencyID = invoiceLine.CurrencyIso4217Code },
                    ///İskonto tutarının uygulandığı tutar
                    BaseAmount = new BaseAmountType { Value = invoiceLine.LineTotalAmount, currencyID = invoiceLine.CurrencyIso4217Code },
                });
            }

            return allowanceChargeTypes.ToArray();
        }

        /// <summary>
        /// Fatura kalemlerini getirir.
        /// </summary>
        /// <param name="faturaKalemleri"></param>
        /// <returns></returns>
        public InvoiceLineType[] GetInvoiceLine(List<InvoiceLine> invoiceLines, string invoiceCurrencyCode)
        {
            List<InvoiceLineType> invoiceLineTypes = new List<InvoiceLineType>();

            foreach (var invoiceLine in invoiceLines)
            {
                invoiceLineTypes.Add(new InvoiceLineType
                {
                    ///Fatura kalem sıra numarası
                    ID = new IDType { Value = invoiceLine.RowNumber.ToString() },
                    ///Fatura Kalem Açıklaması
                    Note = new[] { new NoteType { Value = invoiceLine.Note } },
                    ///Fatura kalem birimi ve miktarı
                    InvoicedQuantity = new InvoicedQuantityType { Value = Convert.ToDecimal(invoiceLine.Quantity), unitCode = invoiceLine.UnitIsoCode },
                    ///Fatura kalem net tutarıdır varsa iskonto düşülür.
                    LineExtensionAmount = new LineExtensionAmountType { Value = invoiceLine.LineTotalAmount + invoiceLine.DiscountPrice, currencyID = invoiceCurrencyCode },
                    ///İskonto veya Arttırım Bilgilerini getirir.
                    AllowanceCharge = GetAllowanceChargeDiscount(invoiceLine),
                    ///Kalem vergi tutarıdır.
                    TaxTotal = GetTaxTotalInvoiceLine(invoiceLine, invoiceCurrencyCode),
                    ///Tevkifat tutarları
                    WithholdingTaxTotal = new[] { GetWithholdingTaxTotalInvoiceLine(invoiceLine, invoiceCurrencyCode) },
                    ///Fatura kalem bilgisidir.
                    Item = new ItemType { Name = new NameType1 { Value = invoiceLine.StockCode + " - " + invoiceLine.StockName } },
                    ///Mal veya hizmetin birim fiyatı
                    Price = new PriceType
                    {
                        PriceAmount = new PriceAmountType { Value = invoiceLine.UnitPrice, currencyID = invoiceCurrencyCode },
                    }
                });
            }
            return invoiceLineTypes.ToArray();
        }

        /// <summary>
        /// Fatura not bilgileridir.
        /// </summary>
        /// <param name="eFaturaParametre"></param>
        /// <returns></returns>
        public NoteType[] GetInvoiceNote(Invoice invoice)
        {
            List<NoteType> noteTypes = new List<NoteType>();
            foreach (var note in invoice.Notes)
                noteTypes.Add(new NoteType { Value = note });
            return noteTypes.ToArray();
        }

        /// <summary>
        /// Mali Mühür / İmza bilgileri
        /// </summary>
        /// <param name="faturaNo"></param>
        /// <returns></returns>
        public SignatureType[] GetSignatureType(Invoice invoice)
        {
            List<SignatureType> signatureType = new List<SignatureType>();
            signatureType.Add(new SignatureType
            {
                ///Firmaya ait vergi numarası
                ID = new IDType { schemeID = "VKN_TCKN", Value = invoice.CustomerInfo.TaxNumber },
                ///İmzalanacak firma bilgileri
                SignatoryParty = GetPartyType(invoice.CustomerInfo),
                ///Bu alana UBLExtensions alanına eklenen dijital imzaya referans eklenecektir.
                DigitalSignatureAttachment = new AttachmentType { ExternalReference = new ExternalReferenceType { URI = new URIType { Value = "#signutura_" + invoice.No } } }
            });

            return signatureType.ToArray();
        }

        /// <summary>
        /// Fatura bilgisindeki vergi tutarını getirir.
        /// </summary>
        /// <param name="faturaKalemDto"></param>
        /// <returns></returns>
        public TaxTotalType[] GetTaxTotal(List<InvoiceLine> invoiceLines, string invoiceCurrencyCode)
        {
            List<Tax> kdvTotalAmount = invoiceLines.GroupBy(g => g.KDVPercent).Select(s => new Tax { Percent = s.Key, Amount = s.Sum(sum => sum.KDVAmount), TaxName = TaxTypes.KDV.ToString(), TaxCode= GetTaxTypeWithCode().Where(w => w.Key == TaxTypes.KDV).FirstOrDefault().Value }).ToList();
            List<Tax> otvTotalAmount = invoiceLines.GroupBy(g => g.OTVPercent).Select(s => new Tax { Percent = s.Key, Amount = s.Sum(sum => sum.OTVAmount), TaxName = TaxTypes.OTV.ToString(), TaxCode = GetTaxTypeWithCode().Where(w => w.Key == TaxTypes.OTV).FirstOrDefault().Value }).ToList();
            List<Tax> oivTotalAmount = invoiceLines.GroupBy(g => g.OIVPercent).Select(s => new Tax { Percent = s.Key, Amount = s.Sum(sum => sum.OIVAmount), TaxName = TaxTypes.OIV.ToString(), TaxCode = GetTaxTypeWithCode().Where(w => w.Key == TaxTypes.OIV).FirstOrDefault().Value }).ToList();
            decimal taxTotalAmount = kdvTotalAmount.Sum(s => s.Amount) + otvTotalAmount.Sum(s => s.Amount) + oivTotalAmount.Sum(s => s.Amount);
            decimal taxableAmount = invoiceLines.Sum(s => s.LineTotalAmount);

            ///Vergilerin ayrıntıları
            List<TaxSubtotalType> taxSubtotalTypes = new List<TaxSubtotalType>();
            taxSubtotalTypes.AddRange(GetTaxSubtotalTypes(kdvTotalAmount, taxSubtotalTypes.Count(), taxableAmount, invoiceCurrencyCode));
            taxSubtotalTypes.AddRange(GetTaxSubtotalTypes(otvTotalAmount, taxSubtotalTypes.Count(), taxableAmount, invoiceCurrencyCode));
            taxSubtotalTypes.AddRange(GetTaxSubtotalTypes(oivTotalAmount, taxSubtotalTypes.Count(), taxableAmount, invoiceCurrencyCode));

            ///Faturadaki vergiler
            List<TaxTotalType> taxTotalTypes = new List<TaxTotalType>();
            taxTotalTypes.Add(
                new TaxTotalType
                {
                    ///Faturaya uygulanan tüm vergilerin toplamı
                    TaxAmount = new TaxAmountType { Value = taxTotalAmount, currencyID = invoiceCurrencyCode },
                    ///Vergilerin ayrıntıları
                    TaxSubtotal = taxSubtotalTypes.ToArray()
                });

            return taxTotalTypes.ToArray();
        }

        /// <summary>
        /// Fatura kalem bilgisindeki vergi tutarlarını getirir.
        /// </summary>
        /// <param name="faturaKalemDto"></param>
        /// <returns></returns>
        public TaxTotalType GetTaxTotalInvoiceLine(InvoiceLine invoiceLine, string invoiceCurrencyCode)
        {
            List<Tax> kdvTotalAmount = (new[] { new Tax { Percent = invoiceLine.KDVPercent, Amount = invoiceLine.KDVAmount, TaxName = TaxTypes.KDV.ToString(), TaxCode = GetTaxTypeWithCode().Where(w => w.Key == TaxTypes.KDV).FirstOrDefault().Value } }).ToList();
            List<Tax> otvTotalAmount = (new[] { new Tax { Percent = invoiceLine.OTVPercent, Amount = invoiceLine.OTVAmount, TaxName = TaxTypes.OTV.ToString(), TaxCode = GetTaxTypeWithCode().Where(w => w.Key == TaxTypes.OTV).FirstOrDefault().Value } }).ToList();
            List<Tax> oivTotalAmount = (new[] { new Tax { Percent = invoiceLine.OIVPercent, Amount = invoiceLine.OIVAmount, TaxName = TaxTypes.OIV.ToString(), TaxCode = GetTaxTypeWithCode().Where(w => w.Key == TaxTypes.OIV).FirstOrDefault().Value } }).ToList();

            decimal taxTotalAmount = kdvTotalAmount.Sum(s => s.Amount) + otvTotalAmount.Sum(s => s.Amount) + oivTotalAmount.Sum(s => s.Amount);
            decimal taxableAmount = invoiceLine.LineTotalAmount;

            ///Vergilerin ayrıntıları
            List<TaxSubtotalType> taxSubtotalTypes = new List<TaxSubtotalType>();
            taxSubtotalTypes.AddRange(GetTaxSubtotalTypes(kdvTotalAmount, taxSubtotalTypes.Count(), taxableAmount, invoiceCurrencyCode));
            taxSubtotalTypes.AddRange(GetTaxSubtotalTypes(otvTotalAmount, taxSubtotalTypes.Count(), taxableAmount, invoiceCurrencyCode));
            taxSubtotalTypes.AddRange(GetTaxSubtotalTypes(oivTotalAmount, taxSubtotalTypes.Count(), taxableAmount, invoiceCurrencyCode));

            ///Fatura kalemine ait vergiler
            TaxTotalType taxTotalType = new TaxTotalType
            {
                ///Kaleme uygulanan tüm vergilerin toplamı
                TaxAmount = new TaxAmountType { Value = taxTotalAmount, currencyID = invoiceCurrencyCode },
                ///Vergilerin ayrıntıları
                TaxSubtotal = taxSubtotalTypes.ToArray()
            };
            return taxTotalType;
        }

        /// <summary>
        /// Fatura bilgisindeki tevkifat bilgisini getirir.
        /// </summary>
        /// <param name="faturaKalemleri"></param>
        /// <param name="faturaCurrencyCode"></param>
        /// <returns></returns>
        public TaxTotalType[] GetWithholdingTaxTotal(List<InvoiceLine> invoiceLines, string invoiceCurrencyCode)
        {
            var withholdingTaxs = invoiceLines.GroupBy(g => new { g.WithholdingTaxCode, g.WithholdingTaxPercent, g.WithholdingTaxName }).Select(s => new Tax { Amount=s.Sum(sum=>sum.WithholdingTaxAmount), Percent=s.Key.WithholdingTaxPercent, TaxCode=s.Key.WithholdingTaxCode,TaxName=s.Key.WithholdingTaxName}).ToList();
            decimal taxTotalAmount = withholdingTaxs.Sum(s => s.Amount);
            decimal taxableAmount = invoiceLines.Sum(s => s.KDVAmount);

            ///Vergilerin ayrıntıları
            List<TaxSubtotalType> taxSubtotalTypes = new List<TaxSubtotalType>();
            taxSubtotalTypes.AddRange(GetTaxSubtotalTypes(withholdingTaxs, taxSubtotalTypes.Count(), taxableAmount, invoiceCurrencyCode));

            ///Faturadaki vergiler
            List<TaxTotalType> taxTotalTypes = new List<TaxTotalType>();
            taxTotalTypes.Add(
                new TaxTotalType
                {
                    ///Faturaya uygulanan tüm vergilerin toplamı
                    TaxAmount = new TaxAmountType { Value = taxTotalAmount, currencyID = invoiceCurrencyCode },
                    ///Vergilerin ayrıntıları
                    TaxSubtotal = taxSubtotalTypes.ToArray()
                });

            return taxTotalTypes.ToArray();
        }

        /// <summary>
        /// Fatura kalem bilgisindeki tevkifat bilgisini getirir.
        /// </summary>
        /// <param name="faturaKalemDto"></param>
        /// <returns></returns>
        public TaxTotalType GetWithholdingTaxTotalInvoiceLine(InvoiceLine invoiceLine, string invoiceCurrencyCode)
        {
            List<Tax> withholdingTaxs = (new[] { new Tax { Percent = invoiceLine.WithholdingTaxPercent, Amount = invoiceLine.WithholdingTaxAmount, TaxName = invoiceLine.WithholdingTaxName, TaxCode=invoiceLine.WithholdingTaxCode } }).ToList();
            decimal taxTotalAmount = withholdingTaxs.Sum(s => s.Amount);
            decimal taxableAmount = invoiceLine.KDVAmount;

            ///Vergilerin ayrıntıları
            List<TaxSubtotalType> taxSubtotalTypes = new List<TaxSubtotalType>();
            taxSubtotalTypes.AddRange(GetTaxSubtotalTypes(withholdingTaxs, taxSubtotalTypes.Count(), taxableAmount, invoiceCurrencyCode));

            ///Fatura kalemine ait vergiler
            TaxTotalType taxTotalType = new TaxTotalType
            {
                ///Kaleme uygulanan tüm vergilerin toplamı
                TaxAmount = new TaxAmountType { Value = taxTotalAmount, currencyID = invoiceCurrencyCode },
                ///Vergilerin ayrıntıları
                TaxSubtotal = taxSubtotalTypes.ToArray()
            };
            return taxTotalType;
        }

        public void GetExampleInvoiceObject()
        {

        }

        public void GetExampleXsltFile()
        {

        }

        /*
        public Invoice Read(byte[] invoice)
        {
            var srl = new XmlSerializer(typeof(UblTr.MainDoc.InvoiceType));
            var ms = new MemoryStream(invoice);
            var ss = (UblTr.MainDoc.InvoiceType)srl.Deserialize(ms);

            Enum.TryParse(ss.ProfileID.Value, out ProfileIds profileId);
            Enum.TryParse(ss.InvoiceTypeCode.Value, out InvoiceTypes invoiceType);
            var res = new Invoice()
            {
                ScenarioType = profileId,
                No = ss.ID.Value,
                Guid = Guid.Parse(ss.UUID.Value),
                Date = ss.IssueDate.Value,
                InvioceType = invoiceType,
                Note1 = ss.Note[0].Value ?? "",
                Note2 = ss.Note[1].Value ?? "",
                Note3 = ss.Note[2].Value ?? "",
                CurrencyIso4217Code = ss.DocumentCurrencyCode.Value,
                CustomerInfo = new CustomerInformation()
                {
                    Name = ss.AccountingSupplierParty.Party.PartyName.Name.Value,
                    TaxNumber = ss.AccountingSupplierParty.Party.PartyIdentification[0].ID.Value ?? "",
                    Room = ss.AccountingSupplierParty.Party.PostalAddress.Room.Value,
                    BlockName = ss.AccountingSupplierParty.Party.PostalAddress.BlockName.Value,
                    BuildingName = ss.AccountingSupplierParty.Party.PostalAddress.BuildingName.Value,
                    BuildingNumber = ss.AccountingSupplierParty.Party.PostalAddress.BuildingNumber.Value,
                    City = ss.AccountingSupplierParty.Party.PostalAddress.CityName.Value,
                    CitySubdivision = ss.AccountingSupplierParty.Party.PostalAddress.CitySubdivisionName.Value,
                    Country = ss.AccountingSupplierParty.Party.PostalAddress.Country.Name.Value,
                    PostalZone = ss.AccountingSupplierParty.Party.PostalAddress.PostalZone.Value,
                    Mail = ss.AccountingSupplierParty.Party.Contact.ElectronicMail.Value,
                    Phone = ss.AccountingSupplierParty.Party.Contact.Telephone.Value,
                },
                InvoiceLine = ss.InvoiceLine.Select(s => new InvoiceLine()
                {
                    Note = s.Note[0].Value ?? "",
                    Amount = s.InvoicedQuantity.Value,
                    UnitIsoCode = s.InvoicedQuantity.unitCode,
                    InvoiceTotalPrice = s.LineExtensionAmount.Value,
                    DiscountPrice = s.AllowanceCharge != null ? s.AllowanceCharge[0].Amount.Value : default,
                    DiscountPercent = s.AllowanceCharge != null ? s.AllowanceCharge[0].MultiplierFactorNumeric.Value : default,
                    KDVPrice = s.TaxTotal.TaxSubtotal.Where(w => w.TaxCategory.TaxScheme.Name.Value == "KDV").Sum(sum => sum.TaxAmount.Value),
                    KDVPercent = s.TaxTotal.TaxSubtotal.Where(w => w.TaxCategory.TaxScheme.Name.Value == "KDV").Sum(sum => sum.Percent.Value),
                    OTVPrice = s.TaxTotal.TaxSubtotal.Where(w => w.TaxCategory.TaxScheme.Name.Value == "OTV").Sum(sum => sum.TaxAmount.Value),
                    OTVPercent = s.TaxTotal.TaxSubtotal.Where(w => w.TaxCategory.TaxScheme.Name.Value == "OTV").Sum(sum => sum.Percent.Value),
                    OIVPrice = s.TaxTotal.TaxSubtotal.Where(w => w.TaxCategory.TaxScheme.Name.Value == "OIV").Sum(sum => sum.TaxAmount.Value),
                    OIVPercent = s.TaxTotal.TaxSubtotal.Where(w => w.TaxCategory.TaxScheme.Name.Value == "OIV").Sum(sum => sum.Percent.Value),
                    UnitPrice = s.Price.PriceAmount.Value,
                    StockName = s.Item.Name.Value,
                }).ToList(),
                Xslt = ss.AdditionalDocumentReference.Where(w => w.DocumentType.Value == "XSLT").Select(s => s.Attachment.EmbeddedDocumentBinaryObject.Value).FirstOrDefault()
            };

            ms.Dispose();
            return res;
        }
        */


        #region Helper Function

        /// <summary>
        /// Vergi bilgilerini subtotaltype listesi olarak hazırlar.
        /// </summary>
        /// <param name="taxs"></param>
        /// <param name="index"></param>
        /// <param name="taxableAmount"></param>
        /// <param name="currencyCode"></param>
        /// <returns></returns>
        private List<TaxSubtotalType> GetTaxSubtotalTypes(List<Tax> taxs, int index, decimal taxableAmount, string currencyCode)
        {
            List<TaxSubtotalType> taxSubtotalTypes = new List<TaxSubtotalType>();
            foreach (var tax in taxs.Where(w => w.Amount > 0))
            {
                taxSubtotalTypes.Add(new TaxSubtotalType
                {
                    ///Vergi Sıra numarası
                    CalculationSequenceNumeric = new CalculationSequenceNumericType { Value = index },
                    ///Matrah tutar bilgisidir.
                    TaxableAmount = new TaxableAmountType { Value = taxableAmount, currencyID = currencyCode },
                    ///Vergi Tutar
                    TaxAmount = new TaxAmountType { Value = tax.Amount, currencyID = currencyCode },
                    ///Uygulanan vergi kodları
                    TaxCategory = new TaxCategoryType
                    {
                        TaxScheme = new TaxSchemeType
                        {
                            Name = new NameType1 { Value = tax.TaxName },
                            TaxTypeCode = new TaxTypeCodeType { Value = tax.TaxCode }
                        },
                    },
                    Percent = new PercentType1 { Value = tax.Percent }
                });
                index++;
            }
            return taxSubtotalTypes;
        }

        /// <summary>
        /// Invoice type objesini Xml'e dönüştürür.
        /// </summary>
        /// <param name="invoiceType"></param>
        /// <returns></returns>
        private string InvoiceTypeToXml(InvoiceType invoiceType)
        {
            var settings = new XmlWriterSettings { OmitXmlDeclaration = true, Indent = true };
            var ms = new MemoryStream();
            var writer = XmlWriter.Create(ms, settings);
            var srl = new XmlSerializer(invoiceType.GetType());
            srl.Serialize(writer, invoiceType);
            ms.Flush();
            ms.Seek(0, SeekOrigin.Begin);
            var srRead = new StreamReader(ms);
            var readXml = srRead.ReadToEnd();

            return readXml;
        }

        /// <summary>
        /// Xml dosyasını diske yazar.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="fileName"></param>
        private void WriteToDiskAsXml(string data, string fileName)
        {
            var path = AppDomain.CurrentDomain.BaseDirectory + "EFaturaTest" + "\\" + fileName + ".xml";
            using (var streamWriter = new StreamWriter(path, append: false, Encoding.UTF8))
            {
                streamWriter.Write(data);
                streamWriter.Close();
            }
        }

        /// <summary>
        /// Vergi kodları
        /// </summary>
        /// <returns></returns>
        public Dictionary<TaxTypes, string> GetTaxTypeWithCode()
        {
            Dictionary<TaxTypes, string> taxTypeKeyValuePairs = new Dictionary<TaxTypes, string>();
            taxTypeKeyValuePairs.Add(TaxTypes.KDV, "0015");
            taxTypeKeyValuePairs.Add(TaxTypes.OTV, "0071");
            taxTypeKeyValuePairs.Add(TaxTypes.OIV, "4080");

            return taxTypeKeyValuePairs;
        }

        #endregion Helper Function
    }
}
