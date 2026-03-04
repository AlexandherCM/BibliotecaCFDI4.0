using System;
using System.Collections.Generic;
using System.Text;

namespace CFDI4._0.ToolsXML.DTOs
{
    public class CancelacionDTO
    {
        public Guid Folio { get; set; }
        public CancelacionFoliosFolioMotivo Motivo { get; set; }
        public Guid? FolioSustitucion { get; set; }
    }   

    //public enum CancelacionMotivo
    //{
    //    /// <summary>
    //    /// Comprobante emitido con errores con relación.
    //    /// </summary>
    //    Motivo01 = 01,
    //    /// <summary>
    //    /// Comprobante emitido con errores sin relación.
    //    /// </summary>
    //    Motivo02 = 02,
    //    /// <summary>
    //    /// No se llevó a cabo la operación.
    //    /// </summary>
    //    Motivo03 = 03,
    //    /// <summary>
    //    /// Operación nominativa relacionada en la factura global.
    //    /// </summary>
    //    Motivo04 = 04
    //}
}
