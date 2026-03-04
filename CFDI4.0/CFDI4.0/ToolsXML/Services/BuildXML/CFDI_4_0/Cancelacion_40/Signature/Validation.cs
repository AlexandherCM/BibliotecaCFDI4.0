using CFDI4._0.ToolsXML.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CFDI4._0.ToolsXML.Services.BuildXML.CFDI_4_0.Cancelacion_40.Signature
{
    public class Validation
    {
        public static void ValidarCancelacion(List<CancelacionDTO> folios)
        {
            if (folios.Count > 500)
            {
                throw new Exception("Se ha excedido el limite de folios a cancelar.");
            }
            if (folios.Any(l => l.Motivo == 0))
            {
                throw new Exception("No se ha especificado un motivo de cancelacion.");
            }
            if (folios.Any(l => l.Motivo == CancelacionFoliosFolioMotivo.Item01 && l.FolioSustitucion is null))
            {
                throw new Exception("El motivo de cancelación no es válido.");
            }
            if (folios.Any(l => l.Folio == Guid.Empty))
            {
                throw new Exception("Los folios no tienen un formato válido.");
            }
        }

        public static void ValidarAceptacionRechazo(List<AceptacionRechazo> folios, string rfcReceptor, byte[] pfx, string password)
        {
            if (String.IsNullOrEmpty(rfcReceptor) || pfx is null || pfx.Length <= 0 || String.IsNullOrEmpty(password))
            {
                throw new Exception("Parametros incompletos.", new Exception("Son necesarios el RFC Receptor, el certificado PFX y la contraseña."));
            }
            if (folios != null && folios.Count > 0)
            {
                if (folios.Count > 500)
                {
                    throw new Exception("Se ha excedido el limite de folios.");
                }
                if (folios.Any(l => l.Folio == Guid.Empty))
                {
                    throw new Exception("Los folios no tienen un formato válido.");
                }
            }
            else
            {
                throw new Exception("Folios inválidos, la lista de folios está vacía o no es correcta.");
            }
        }

        public class AceptacionRechazo
        {
            public Guid Folio { get; set; }
            public AceptacionRechazoRespuesta Respuesta { get; set; }
        }

        public enum AceptacionRechazoRespuesta
        {
            /// <summary>
            /// Aceptar cancelación.
            /// </summary>
            Aceptacion,
            /// <summary>
            /// Rechazar cancelación.
            /// </summary>
            Rechazo
        }
    }
}
