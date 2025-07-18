using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using CFDI4._0.CFDI;
using CFDI4._0.CFDI.EsquemaCFDI3_3;
using CFDI4._0.ToolsXML.CFDI_3_3;

namespace CFDI4._0.ToolsXML.ProcessCFDI
{
    public class ScanXML
    {
        private string _XmlContent = string.Empty;
        private OpCFDI opCFDI;
        private OpCFDI_3_3 opCFDI_3_3;      

        public ScanXML()
        {
            opCFDI = new OpCFDI();
            opCFDI_3_3 = new OpCFDI_3_3();  
        }

        public Comprobante ObtenerDatosCFDI4_0()
            => opCFDI.DeserializarXMLCompleto(_XmlContent);
        
        public Comprobante3_3 ObtenerDatosCFDI3_3()
            => opCFDI_3_3.DeserializarXMLCompleto(_XmlContent);

        public float GetVersionFromXml(string xmlContent)         
        {
            _XmlContent = xmlContent;

            if (string.IsNullOrWhiteSpace(_XmlContent))
                throw new ArgumentException("El contenido del XML no puede estar vacío.");

            var xmlDoc = new XmlDocument(); 
            xmlDoc.LoadXml(_XmlContent);

            XmlElement root = xmlDoc.DocumentElement;

            if (root == null || root.LocalName != "Comprobante")
                throw new InvalidOperationException("No se encontró el nodo Comprobante en el XML.");

            string versionStr = root.GetAttribute("Version");

            if (string.IsNullOrWhiteSpace(versionStr))
                throw new InvalidOperationException("El atributo 'Version' no está presente.");

            if (float.TryParse(versionStr, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out float version))
                return version;

            throw new FormatException($"El atributo 'Version' no tiene un formato numérico válido: '{versionStr}'.");
        }

        
    }
}
