using System.Xml.Serialization;

namespace CFDI4._0.CFDI
{
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.8.3928.0")]
    [System.Serializable()]
    [System.Diagnostics.DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://www.sat.gob.mx/TimbreFiscalDigital")]
    [XmlRoot(Namespace = "http://www.sat.gob.mx/TimbreFiscalDigital", IsNullable = false)]
    public partial class TimbreFiscalDigital
    {

        private string versionField;

        private string uUIDField;

        private System.DateTime fechaTimbradoField;

        private string rfcProvCertifField;

        private string leyendaField;

        private string selloCFDField;

        private string noCertificadoSATField;

        private string selloSATField;

        public TimbreFiscalDigital()
        {
            versionField = "1.1";
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

        /// <remarks/>
        [XmlAttribute()]
        public System.DateTime FechaTimbrado
        {
            get
            {
                return fechaTimbradoField;
            }
            set
            {
                fechaTimbradoField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string RfcProvCertif
        {
            get
            {
                return rfcProvCertifField;
            }
            set
            {
                rfcProvCertifField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string Leyenda
        {
            get
            {
                return leyendaField;
            }
            set
            {
                leyendaField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string SelloCFD
        {
            get
            {
                return selloCFDField;
            }
            set
            {
                selloCFDField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string NoCertificadoSAT
        {
            get
            {
                return noCertificadoSATField;
            }
            set
            {
                noCertificadoSATField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string SelloSAT
        {
            get
            {
                return selloSATField;
            }
            set
            {
                selloSATField = value;
            }
        }
    }
}
