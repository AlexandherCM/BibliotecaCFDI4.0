using CFDI4._0.CFDI;
using System;
using System.Collections.Generic;
using System.Text;

namespace CFDI4._0.ToolsXML
{
    public static class InitObj
    {
        // FUNCIONES PARA INICIALIZAR EL OBJETO COMPROBANTE - - - - - - - - - - - - - - - - - - - - - - - - 
        public static Comprobante IniciarObjComprobanteP()
        {
            var oComprobante = new Comprobante()
            {
                // Se agrega la definición correcta del esquema de Pagos 2.0
                xsiSchemaLocation = "http://www.sat.gob.mx/cfd/4 http://www.sat.gob.mx/sitio_internet/cfd/4/cfdv40.xsd " +
                                    "http://www.sat.gob.mx/Pagos20 http://www.sat.gob.mx/sitio_internet/cfd/Pagos/Pagos20.xsd",
                Version = "4.0",
                Fecha = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss"),
                SubTotal = 0m,
                Total = 0m,
                Moneda = "XXX",
                Exportacion = "01",
                TipoDeComprobante = "P",
                Conceptos = new[]
                {
                    new ComprobanteConcepto
                    {
                        ClaveProdServ = "84111506",
                        ClaveUnidad = "ACT",
                        Cantidad = 1,
                        Descripcion = "Pago",
                        ValorUnitario = 0m,
                        Importe = 0m,
                        ObjetoImp = "01"
                    }
                },
                // - - - - - - - - - - - - - - - - - - - - - - - -
                Complemento = new ComprobanteComplemento(),
                // - - - - - - - - - - - - - - - - - - - - - - - -
                complementoPagos = new Pagos()
                {
                    Version = "2.0",
                    Totales = new PagosTotales()
                    {
                        TotalTrasladosBaseIVA16Specified = true,
                        TotalTrasladosImpuestoIVA16Specified = true
                    }
                    //Pago = new PagosPago[] {  }
                }
                // - - - - - - - - - - - - - - - - - - - - - - - -
            };

            return oComprobante;
        }

        public static PagosPago CrearPago (string FormaDePagoP, decimal Total, decimal SubTotal, decimal ImporteIva, string UUID, string Serie, string Folio, string NumOperacion = "")
        {   
            // - - - - - - - - - - - - - - - - - - - - - - - -  
            var pago = new PagosPago
            {
                FechaPago = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss"),
                FormaDePagoP = FormaDePagoP,
                MonedaP = "MXN",
                TipoCambioP = 1m,
                TipoCambioPSpecified = true,
                Monto = Total,
                // Se añade impuestos trasladados al pago
                // - - - - - - - - - - - - - - - - - - - - - - - -
                ImpuestosP = new PagosPagoImpuestosP
                {
                    TrasladosP = new PagosPagoImpuestosPTrasladoP[]
                    {
                            new PagosPagoImpuestosPTrasladoP
                            {
                                BaseP = SubTotal,
                                ImpuestoP = "002",
                                TipoFactorP = "Tasa",
                                TasaOCuotaP = 0.160000m,
                                TasaOCuotaPSpecified = true,
                                ImporteP = ImporteIva,
                                ImportePSpecified = true
                            }
                    }
                },
                // - - - - - - - - - - - - - - - - - - - - - - - -
                DoctoRelacionado = new PagosPagoDoctoRelacionado[]
                {
                         // - - - - - - - - - - - - - - - - - - - - - - - -
                        new PagosPagoDoctoRelacionado
                        {
                            IdDocumento = UUID,
                            Serie = Serie,
                            Folio = Folio,
                            MonedaDR = "MXN",
                            NumParcialidad = "1",
                            ImpSaldoAnt = Total,
                            ImpPagado = Total,
                            ImpSaldoInsoluto = 0.00m,
                            ObjetoImpDR = "02",
                            EquivalenciaDR = 1m, // ✅ Asegurado que se escriba en el XML
                            EquivalenciaDRSpecified = true,
                             // - - - - - - - - - - - - - - - - - - - - - - - -
                            ImpuestosDR = new PagosPagoDoctoRelacionadoImpuestosDR
                            {
                                TrasladosDR = new PagosPagoDoctoRelacionadoImpuestosDRTrasladoDR[]
                                {
                                    // - - - - - - - - - - - - - - - - - - - - - - - -
                                    new PagosPagoDoctoRelacionadoImpuestosDRTrasladoDR
                                    {
                                        BaseDR = SubTotal,
                                        ImpuestoDR = "002",
                                        TipoFactorDR = "Tasa",
                                        TasaOCuotaDR = 0.160000m,
                                        TasaOCuotaDRSpecified = true,
                                        ImporteDR = ImporteIva,
                                        ImporteDRSpecified = true
                                    }
                                }
                                // - - - - - - - - - - - - - - - - - - - - - - - -
                            }
                            // - - - - - - - - - - - - - - - - - - - - - - - -
                        }
                        // - - - - - - - - - - - - - - - - - - - - - - - -
                }
                // - - - - - - - - - - - - - - - - - - - - - - - -
            };

            if(!string.IsNullOrEmpty(NumOperacion)) pago.NumOperacion = NumOperacion;
            return pago;
        }


    }
}
