using System;
using System.IO;
using System.Xml.Schema;
using System.Xml.Serialization;
using CFDI4._0.ToolsPDF;

namespace CFDI4._0.CFDI
{
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.8.3928.0")]
    [Serializable()]
    [System.Diagnostics.DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://www.sat.gob.mx/cfd/4")]
    [XmlRoot(Namespace = "http://www.sat.gob.mx/cfd/4", IsNullable = false)]
    public partial class Comprobante
    {
        private ComprobanteInformacionGlobal informacionGlobalField;

        private ComprobanteCfdiRelacionados[] cfdiRelacionadosField;

        private ComprobanteEmisor emisorField;

        private ComprobanteReceptor receptorField;

        private ComprobanteConcepto[] conceptosField;

        private ComprobanteImpuestos impuestosField;

        private ComprobanteComplemento complementoField;

        private ComprobanteAddenda addendaField;

        public TimbreFiscalDigital TimbreFiscalDigital;

        private string versionField;

        private string serieField;

        private string folioField;

        private string fechaField;

        private string selloField;

        private string formaPagoField;

        private bool formaPagoFieldSpecified;

        private string noCertificadoField;

        private string certificadoField;

        private string condicionesDePagoField;

        private decimal subTotalField;

        private decimal descuentoField;

        private bool descuentoFieldSpecified;

        private string monedaField;

        private decimal tipoCambioField;

        private bool tipoCambioFieldSpecified;

        private decimal totalField;

        private string tipoDeComprobanteField;

        private string exportacionField;

        private string metodoPagoField;

        private bool metodoPagoFieldSpecified;

        private string lugarExpedicionField;

        private string confirmacionField;

        [XmlIgnore]
        public string CadenaOriginal;

        public string QR
        {
            get
            {
                // Generar el hash 'fe' a partir del sello (últimos 8 caracteres, Base64)
                string selloRecortado = Sello.Substring(Sello.Length - 8);  // Extrae los últimos 8 caracteres
                byte[] selloBytes = System.Text.Encoding.UTF8.GetBytes(selloRecortado);
                string feHash = Convert.ToBase64String(selloBytes).Replace("=", "");  // Eliminar el relleno '=' si existe

                // Generar la URL del SAT
                string urlSAT = $"https://verificacfdi.facturaelectronica.sat.gob.mx/default.aspx?id={TimbreFiscalDigital.UUID}" +
                                       $"&re={Emisor.Rfc}&rr={Receptor.Rfc}&tt={Total.ToString("F6")}&fe={feHash}";

                // Generar el código QR a partir de la URL
                return Qr.GenerateQRCode(urlSAT);
            }
        }

        public string MonedaConLetra
        {
            get
            {
                NumberToLetters oMoneda = new NumberToLetters();
                return oMoneda.Convertir(Total.ToString("#.00"), true);
            }
        }

        [XmlAttribute("schemaLocation", Namespace = XmlSchema.InstanceNamespace)]
        public string xsiSchemaLocation = "http://www.sat.gob.mx/cfd/4 http://www.sat.gob.mx/sitio_internet/cfd/4/cfdv40.xsd";

        public Comprobante()
        {
            versionField = "4.0";
        }

        /// <remarks/>
        public ComprobanteInformacionGlobal InformacionGlobal
        {
            get
            {
                return informacionGlobalField;
            }
            set
            {
                informacionGlobalField = value;
            }
        }

        /// <remarks/>
        [XmlElement("CfdiRelacionados")]
        public ComprobanteCfdiRelacionados[] CfdiRelacionados
        {
            get
            {
                return cfdiRelacionadosField;
            }
            set
            {
                cfdiRelacionadosField = value;
            }
        }

        /// <remarks/>
        public ComprobanteEmisor Emisor
        {
            get
            {
                return emisorField;
            }
            set
            {
                emisorField = value;
            }
        }

        /// <remarks/>
        public ComprobanteReceptor Receptor
        {
            get
            {
                return receptorField;
            }
            set
            {
                receptorField = value;
            }
        }

        /// <remarks/>
        [XmlArrayItem("Concepto", IsNullable = false)]
        public ComprobanteConcepto[] Conceptos
        {
            get
            {
                return conceptosField;
            }
            set
            {
                conceptosField = value;
            }
        }

        /// <remarks/>
        public ComprobanteImpuestos Impuestos
        {
            get
            {
                return impuestosField;
            }
            set
            {
                impuestosField = value;
            }
        }

        /// <remarks/>
        public ComprobanteComplemento Complemento
        {
            get
            {
                return complementoField;
            }
            set
            {
                complementoField = value;
            }
        }

        /// <remarks/>
        public ComprobanteAddenda Addenda
        {
            get
            {
                return addendaField;
            }
            set
            {
                addendaField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string Version
        {
            get
            {
                return versionField;
            }
            set
            {
                versionField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string Serie
        {
            get
            {
                return serieField;
            }
            set
            {
                serieField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string Folio
        {
            get
            {
                return folioField;
            }
            set
            {
                folioField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string Fecha
        {
            get
            {
                return fechaField;
            }
            set
            {
                fechaField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string Sello
        {
            get
            {
                return selloField;
            }
            set
            {
                selloField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string FormaPago
        {
            get
            {
                return formaPagoField;
            }
            set
            {
                formaPagoFieldSpecified = true;
                formaPagoField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool FormaPagoSpecified
        {
            get
            {
                return formaPagoFieldSpecified;
            }
            set
            {
                formaPagoFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string NoCertificado
        {
            get
            {
                return noCertificadoField;
            }
            set
            {
                noCertificadoField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string Certificado
        {
            get
            {
                return certificadoField;
            }
            set
            {
                certificadoField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string CondicionesDePago
        {
            get
            {
                return condicionesDePagoField;
            }
            set
            {
                condicionesDePagoField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public decimal SubTotal
        {
            get
            {
                return subTotalField;
            }
            set
            {
                subTotalField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public decimal Descuento
        {
            get
            {
                return descuentoField;
            }
            set
            {
                descuentoFieldSpecified = true;
                descuentoField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool DescuentoSpecified
        {
            get
            {
                return descuentoFieldSpecified;
            }
            set
            {
                descuentoFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string Moneda
        {
            get
            {
                return monedaField;
            }
            set
            {
                monedaField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public decimal TipoCambio
        {
            get
            {
                return tipoCambioField;
            }
            set
            {
                tipoCambioField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool TipoCambioSpecified
        {
            get
            {
                return tipoCambioFieldSpecified;
            }
            set
            {
                tipoCambioFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public decimal Total
        {
            get
            {
                return totalField;
            }
            set
            {
                totalField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string TipoDeComprobante
        {
            get
            {
                return tipoDeComprobanteField;
            }
            set
            {
                tipoDeComprobanteField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string Exportacion
        {
            get
            {
                return exportacionField;
            }
            set
            {
                exportacionField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string MetodoPago
        {
            get
            {
                return metodoPagoField;
            }
            set
            {
                metodoPagoFieldSpecified = true;
                metodoPagoField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool MetodoPagoSpecified
        {
            get
            {
                return metodoPagoFieldSpecified;
            }
            set
            {
                metodoPagoFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string LugarExpedicion
        {
            get
            {
                return lugarExpedicionField;
            }
            set
            {
                lugarExpedicionField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string Confirmacion
        {
            get
            {
                return confirmacionField;
            }
            set
            {
                confirmacionField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.8.3928.0")]
    [Serializable()]
    [System.Diagnostics.DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://www.sat.gob.mx/cfd/4")]
    public partial class ComprobanteInformacionGlobal
    {

        private string periodicidadField;

        private string mesesField;

        private short añoField;

        /// <remarks/>
        [XmlAttribute()]
        public string Periodicidad
        {
            get
            {
                return periodicidadField;
            }
            set
            {
                periodicidadField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string Meses
        {
            get
            {
                return mesesField;
            }
            set
            {
                mesesField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public short Año
        {
            get
            {
                return añoField;
            }
            set
            {
                añoField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.8.3928.0")]
    [Serializable()]
    [System.Diagnostics.DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://www.sat.gob.mx/cfd/4")]
    public partial class ComprobanteCfdiRelacionados
    {

        private ComprobanteCfdiRelacionadosCfdiRelacionado[] cfdiRelacionadoField;

        private string tipoRelacionField;

        /// <remarks/>
        [XmlElement("CfdiRelacionado")]
        public ComprobanteCfdiRelacionadosCfdiRelacionado[] CfdiRelacionado
        {
            get
            {
                return cfdiRelacionadoField;
            }
            set
            {
                cfdiRelacionadoField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string TipoRelacion
        {
            get
            {
                return tipoRelacionField;
            }
            set
            {
                tipoRelacionField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.8.3928.0")]
    [Serializable()]
    [System.Diagnostics.DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://www.sat.gob.mx/cfd/4")]
    public partial class ComprobanteCfdiRelacionadosCfdiRelacionado
    {

        private string uUIDField;

        /// <remarks/>
        [XmlAttribute()]
        public string UUID
        {
            get
            {
                return uUIDField;
            }
            set
            {
                uUIDField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.8.3928.0")]
    [Serializable()]
    [System.Diagnostics.DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://www.sat.gob.mx/cfd/4")]
    public partial class ComprobanteEmisor
    {

        private string rfcField;

        private string nombreField;

        private string regimenFiscalField;

        private string facAtrAdquirenteField;

        /// <remarks/>
        [XmlAttribute()]
        public string Rfc
        {
            get
            {
                return rfcField;
            }
            set
            {
                rfcField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string Nombre
        {
            get
            {
                return nombreField;
            }
            set
            {
                nombreField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string RegimenFiscal
        {
            get
            {
                return regimenFiscalField;
            }
            set
            {
                regimenFiscalField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string FacAtrAdquirente
        {
            get
            {
                return facAtrAdquirenteField;
            }
            set
            {
                facAtrAdquirenteField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.8.3928.0")]
    [Serializable()]
    [System.Diagnostics.DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://www.sat.gob.mx/cfd/4")]
    public partial class ComprobanteReceptor
    {

        private string rfcField;

        private string nombreField;

        private string domicilioFiscalReceptorField;

        private string residenciaFiscalField;

        private bool residenciaFiscalFieldSpecified;

        private string numRegIdTribField;

        private string regimenFiscalReceptorField;

        private string usoCFDIField;

        /// <remarks/>
        [XmlAttribute()]
        public string Rfc
        {
            get
            {
                return rfcField;
            }
            set
            {
                rfcField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string Nombre
        {
            get
            {
                return nombreField;
            }
            set
            {
                nombreField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute("DomicilioFiscalReceptor")]
        public string DomicilioFiscalReceptor
        {
            get
            {
                return domicilioFiscalReceptorField;
            }
            set
            {
                domicilioFiscalReceptorField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string ResidenciaFiscal
        {
            get
            {
                return residenciaFiscalField;
            }
            set
            {
                residenciaFiscalField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool ResidenciaFiscalSpecified
        {
            get
            {
                return residenciaFiscalFieldSpecified;
            }
            set
            {
                residenciaFiscalFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string NumRegIdTrib
        {
            get
            {
                return numRegIdTribField;
            }
            set
            {
                numRegIdTribField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string RegimenFiscalReceptor
        {
            get
            {
                return regimenFiscalReceptorField;
            }
            set
            {
                regimenFiscalReceptorField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string UsoCFDI
        {
            get
            {
                return usoCFDIField;
            }
            set
            {
                usoCFDIField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.8.3928.0")]
    [Serializable()]
    [System.Diagnostics.DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://www.sat.gob.mx/cfd/4")]
    public partial class ComprobanteConcepto
    {

        private ComprobanteConceptoImpuestos impuestosField;

        private ComprobanteConceptoACuentaTerceros aCuentaTercerosField;

        private ComprobanteConceptoInformacionAduanera[] informacionAduaneraField;

        private ComprobanteConceptoCuentaPredial[] cuentaPredialField;

        private ComprobanteConceptoComplementoConcepto complementoConceptoField;

        private ComprobanteConceptoParte[] parteField;

        private string claveProdServField;

        private string noIdentificacionField;

        private decimal cantidadField;

        private string claveUnidadField;

        private string unidadField;

        private string descripcionField;

        private decimal valorUnitarioField;

        private decimal importeField;

        private decimal descuentoField;

        private bool descuentoFieldSpecified;

        private string objetoImpField;

        /// <remarks/>
        public ComprobanteConceptoImpuestos Impuestos
        {
            get
            {
                return impuestosField;
            }
            set
            {
                impuestosField = value;
            }
        }

        /// <remarks/>
        public ComprobanteConceptoACuentaTerceros ACuentaTerceros
        {
            get
            {
                return aCuentaTercerosField;
            }
            set
            {
                aCuentaTercerosField = value;
            }
        }

        /// <remarks/>
        [XmlElement("InformacionAduanera")]
        public ComprobanteConceptoInformacionAduanera[] InformacionAduanera
        {
            get
            {
                return informacionAduaneraField;
            }
            set
            {
                informacionAduaneraField = value;
            }
        }

        /// <remarks/>
        [XmlElement("CuentaPredial")]
        public ComprobanteConceptoCuentaPredial[] CuentaPredial
        {
            get
            {
                return cuentaPredialField;
            }
            set
            {
                cuentaPredialField = value;
            }
        }

        /// <remarks/>
        public ComprobanteConceptoComplementoConcepto ComplementoConcepto
        {
            get
            {
                return complementoConceptoField;
            }
            set
            {
                complementoConceptoField = value;
            }
        }

        /// <remarks/>
        [XmlElement("Parte")]
        public ComprobanteConceptoParte[] Parte
        {
            get
            {
                return parteField;
            }
            set
            {
                parteField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string ClaveProdServ
        {
            get
            {
                return claveProdServField;
            }
            set
            {
                claveProdServField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string NoIdentificacion
        {
            get
            {
                return noIdentificacionField;
            }
            set
            {
                noIdentificacionField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public decimal Cantidad
        {
            get
            {
                return cantidadField;
            }
            set
            {
                cantidadField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string ClaveUnidad
        {
            get
            {
                return claveUnidadField;
            }
            set
            {
                claveUnidadField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string Unidad
        {
            get
            {
                return unidadField;
            }
            set
            {
                unidadField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string Descripcion
        {
            get
            {
                return descripcionField;
            }
            set
            {
                descripcionField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public decimal ValorUnitario
        {
            get
            {
                return valorUnitarioField;
            }
            set
            {
                valorUnitarioField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public decimal Importe
        {
            get
            {
                return importeField;
            }
            set
            {
                importeField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public decimal Descuento
        {
            get
            {
                return descuentoField;
            }
            set
            {
                descuentoFieldSpecified = true;
                descuentoField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool DescuentoSpecified
        {
            get
            {
                return descuentoFieldSpecified;
            }
            set
            {
                descuentoFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string ObjetoImp
        {
            get
            {
                return objetoImpField;
            }
            set
            {
                objetoImpField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.8.3928.0")]
    [Serializable()]
    [System.Diagnostics.DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://www.sat.gob.mx/cfd/4")]
    public partial class ComprobanteConceptoImpuestos
    {

        private ComprobanteConceptoImpuestosTraslado[] trasladosField;

        private ComprobanteConceptoImpuestosRetencion[] retencionesField;

        /// <remarks/>
        [XmlArrayItem("Traslado", IsNullable = false)]
        public ComprobanteConceptoImpuestosTraslado[] Traslados
        {
            get
            {
                return trasladosField;
            }
            set
            {
                trasladosField = value;
            }
        }

        /// <remarks/>
        [XmlArrayItem("Retencion", IsNullable = false)]
        public ComprobanteConceptoImpuestosRetencion[] Retenciones
        {
            get
            {
                return retencionesField;
            }
            set
            {
                retencionesField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.8.3928.0")]
    [Serializable()]
    [System.Diagnostics.DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://www.sat.gob.mx/cfd/4")]
    public partial class ComprobanteConceptoImpuestosTraslado
    {

        private decimal baseField;

        private string impuestoField;

        private string tipoFactorField;

        private decimal tasaOCuotaField;

        private bool tasaOCuotaFieldSpecified;

        private decimal importeField;

        private bool importeFieldSpecified;

        /// <remarks/>
        [XmlAttribute()]
        public decimal Base
        {
            get
            {
                return baseField;
            }
            set
            {
                baseField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string Impuesto
        {
            get
            {
                return impuestoField;
            }
            set
            {
                impuestoField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string TipoFactor
        {
            get
            {
                return tipoFactorField;
            }
            set
            {
                tipoFactorField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public decimal TasaOCuota
        {
            get
            {
                return tasaOCuotaField;
            }
            set
            {
                tasaOCuotaFieldSpecified = true;
                tasaOCuotaField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool TasaOCuotaSpecified
        {
            get
            {
                return tasaOCuotaFieldSpecified;
            }
            set
            {
                tasaOCuotaFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public decimal Importe
        {
            get
            {
                return importeField;
            }
            set
            {
                importeFieldSpecified = true;
                importeField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool ImporteSpecified
        {
            get
            {
                return importeFieldSpecified;
            }
            set
            {
                importeFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.8.3928.0")]
    [Serializable()]
    [System.Diagnostics.DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://www.sat.gob.mx/cfd/4")]
    public partial class ComprobanteConceptoImpuestosRetencion
    {

        private decimal baseField;

        private string impuestoField;

        private string tipoFactorField;

        private decimal tasaOCuotaField;

        private decimal importeField;

        /// <remarks/>
        [XmlAttribute()]
        public decimal Base
        {
            get
            {
                return baseField;
            }
            set
            {
                baseField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string Impuesto
        {
            get
            {
                return impuestoField;
            }
            set
            {
                impuestoField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string TipoFactor
        {
            get
            {
                return tipoFactorField;
            }
            set
            {
                tipoFactorField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public decimal TasaOCuota
        {
            get
            {
                return tasaOCuotaField;
            }
            set
            {
                tasaOCuotaField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public decimal Importe
        {
            get
            {
                return importeField;
            }
            set
            {
                importeField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.8.3928.0")]
    [Serializable()]
    [System.Diagnostics.DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://www.sat.gob.mx/cfd/4")]
    public partial class ComprobanteConceptoACuentaTerceros
    {

        private string rfcACuentaTercerosField;

        private string nombreACuentaTercerosField;

        private string regimenFiscalACuentaTercerosField;

        private string domicilioFiscalACuentaTercerosField;

        /// <remarks/>
        [XmlAttribute()]
        public string RfcACuentaTerceros
        {
            get
            {
                return rfcACuentaTercerosField;
            }
            set
            {
                rfcACuentaTercerosField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string NombreACuentaTerceros
        {
            get
            {
                return nombreACuentaTercerosField;
            }
            set
            {
                nombreACuentaTercerosField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string RegimenFiscalACuentaTerceros
        {
            get
            {
                return regimenFiscalACuentaTercerosField;
            }
            set
            {
                regimenFiscalACuentaTercerosField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string DomicilioFiscalACuentaTerceros
        {
            get
            {
                return domicilioFiscalACuentaTercerosField;
            }
            set
            {
                domicilioFiscalACuentaTercerosField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.8.3928.0")]
    [Serializable()]
    [System.Diagnostics.DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://www.sat.gob.mx/cfd/4")]
    public partial class ComprobanteConceptoInformacionAduanera
    {

        private string numeroPedimentoField;

        /// <remarks/>
        [XmlAttribute()]
        public string NumeroPedimento
        {
            get
            {
                return numeroPedimentoField;
            }
            set
            {
                numeroPedimentoField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.8.3928.0")]
    [Serializable()]
    [System.Diagnostics.DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://www.sat.gob.mx/cfd/4")]
    public partial class ComprobanteConceptoCuentaPredial
    {

        private string numeroField;

        /// <remarks/>
        [XmlAttribute()]
        public string Numero
        {
            get
            {
                return numeroField;
            }
            set
            {
                numeroField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.8.3928.0")]
    [Serializable()]
    [System.Diagnostics.DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://www.sat.gob.mx/cfd/4")]
    public partial class ComprobanteConceptoComplementoConcepto
    {

        private System.Xml.XmlElement[] anyField;

        /// <remarks/>
        [XmlAnyElement()]
        public System.Xml.XmlElement[] Any
        {
            get
            {
                return anyField;
            }
            set
            {
                anyField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.8.3928.0")]
    [Serializable()]
    [System.Diagnostics.DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://www.sat.gob.mx/cfd/4")]
    public partial class ComprobanteConceptoParte
    {

        private ComprobanteConceptoParteInformacionAduanera[] informacionAduaneraField;

        private string claveProdServField;

        private string noIdentificacionField;

        private decimal cantidadField;

        private string unidadField;

        private string descripcionField;

        private decimal valorUnitarioField;

        private bool valorUnitarioFieldSpecified;

        private decimal importeField;

        private bool importeFieldSpecified;

        /// <remarks/>
        [XmlElement("InformacionAduanera")]
        public ComprobanteConceptoParteInformacionAduanera[] InformacionAduanera
        {
            get
            {
                return informacionAduaneraField;
            }
            set
            {
                informacionAduaneraField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string ClaveProdServ
        {
            get
            {
                return claveProdServField;
            }
            set
            {
                claveProdServField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string NoIdentificacion
        {
            get
            {
                return noIdentificacionField;
            }
            set
            {
                noIdentificacionField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public decimal Cantidad
        {
            get
            {
                return cantidadField;
            }
            set
            {
                cantidadField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string Unidad
        {
            get
            {
                return unidadField;
            }
            set
            {
                unidadField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string Descripcion
        {
            get
            {
                return descripcionField;
            }
            set
            {
                descripcionField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public decimal ValorUnitario
        {
            get
            {
                return valorUnitarioField;
            }
            set
            {
                valorUnitarioField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool ValorUnitarioSpecified
        {
            get
            {
                return valorUnitarioFieldSpecified;
            }
            set
            {
                valorUnitarioFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public decimal Importe
        {
            get
            {
                return importeField;
            }
            set
            {
                importeField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool ImporteSpecified
        {
            get
            {
                return importeFieldSpecified;
            }
            set
            {
                importeFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.8.3928.0")]
    [Serializable()]
    [System.Diagnostics.DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://www.sat.gob.mx/cfd/4")]
    public partial class ComprobanteConceptoParteInformacionAduanera
    {

        private string numeroPedimentoField;

        /// <remarks/>
        [XmlAttribute()]
        public string NumeroPedimento
        {
            get
            {
                return numeroPedimentoField;
            }
            set
            {
                numeroPedimentoField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.8.3928.0")]
    [Serializable()]
    [System.Diagnostics.DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://www.sat.gob.mx/cfd/4")]
    public partial class ComprobanteImpuestos
    {

        private ComprobanteImpuestosRetencion[] retencionesField;

        private ComprobanteImpuestosTraslado[] trasladosField;

        private decimal totalImpuestosRetenidosField;

        private bool totalImpuestosRetenidosFieldSpecified;

        private decimal totalImpuestosTrasladadosField;

        private bool totalImpuestosTrasladadosFieldSpecified;

        /// <remarks/>
        [XmlArrayItem("Retencion", IsNullable = false)]
        public ComprobanteImpuestosRetencion[] Retenciones
        {
            get
            {
                return retencionesField;
            }
            set
            {
                retencionesField = value;
            }
        }

        /// <remarks/>
        [XmlArrayItem("Traslado", IsNullable = false)]
        public ComprobanteImpuestosTraslado[] Traslados
        {
            get
            {
                return trasladosField;
            }
            set
            {
                trasladosField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public decimal TotalImpuestosRetenidos
        {
            get
            {
                return totalImpuestosRetenidosField;
            }
            set
            {
                totalImpuestosRetenidosField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool TotalImpuestosRetenidosSpecified
        {
            get
            {
                return totalImpuestosRetenidosFieldSpecified;
            }
            set
            {
                totalImpuestosRetenidosFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public decimal TotalImpuestosTrasladados
        {
            get
            {
                return totalImpuestosTrasladadosField;
            }
            set
            {
                TotalImpuestosTrasladadosSpecified = true;
                totalImpuestosTrasladadosField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool TotalImpuestosTrasladadosSpecified
        {
            get
            {
                return totalImpuestosTrasladadosFieldSpecified;
            }
            set
            {
                totalImpuestosTrasladadosFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.8.3928.0")]
    [Serializable()]
    [System.Diagnostics.DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://www.sat.gob.mx/cfd/4")]
    public partial class ComprobanteImpuestosRetencion
    {

        private string impuestoField;

        private decimal importeField;

        /// <remarks/>
        [XmlAttribute()]
        public string Impuesto
        {
            get
            {
                return impuestoField;
            }
            set
            {
                impuestoField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public decimal Importe
        {
            get
            {
                return importeField;
            }
            set
            {
                importeField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.8.3928.0")]
    [Serializable()]
    [System.Diagnostics.DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://www.sat.gob.mx/cfd/4")]
    public partial class ComprobanteImpuestosTraslado
    {

        private decimal baseField;

        private string impuestoField;

        private string tipoFactorField;

        private decimal tasaOCuotaField;

        private bool tasaOCuotaFieldSpecified;

        private decimal importeField;

        private bool importeFieldSpecified;

        /// <remarks/>
        [XmlAttribute()]
        public decimal Base
        {
            get
            {
                return baseField;
            }
            set
            {
                baseField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string Impuesto
        {
            get
            {
                return impuestoField;
            }
            set
            {
                impuestoField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string TipoFactor
        {
            get
            {
                return tipoFactorField;
            }
            set
            {
                tipoFactorField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public decimal TasaOCuota
        {
            get
            {
                return tasaOCuotaField;
            }
            set
            {
                TasaOCuotaSpecified = true;
                tasaOCuotaField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool TasaOCuotaSpecified
        {
            get
            {
                return tasaOCuotaFieldSpecified;
            }
            set
            {
                tasaOCuotaFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public decimal Importe
        {
            get
            {
                return importeField;
            }
            set
            {
                ImporteSpecified = true;
                importeField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool ImporteSpecified
        {
            get
            {
                return importeFieldSpecified;
            }
            set
            {
                importeFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.8.3928.0")]
    [Serializable()]
    [System.Diagnostics.DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://www.sat.gob.mx/cfd/4")]
    public partial class ComprobanteComplemento
    {

        private System.Xml.XmlElement[] anyField;

        /// <remarks/>
        [XmlAnyElement()]
        public System.Xml.XmlElement[] Any
        {
            get
            {
                return anyField;
            }
            set
            {
                anyField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.8.3928.0")]
    [Serializable()]
    [System.Diagnostics.DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://www.sat.gob.mx/cfd/4")]
    public partial class ComprobanteAddenda
    {

        private System.Xml.XmlElement[] anyField;

        /// <remarks/>
        [XmlAnyElement()]
        public System.Xml.XmlElement[] Any
        {
            get
            {
                return anyField;
            }
            set
            {
                anyField = value;
            }
        }
    }
}
