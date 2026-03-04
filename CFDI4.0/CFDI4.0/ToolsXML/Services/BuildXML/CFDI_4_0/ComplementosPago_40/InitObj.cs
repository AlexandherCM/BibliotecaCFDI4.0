using CFDI4._0.CFDI;
using System;
using System.Collections.Generic;
using System.Text;

namespace CFDI4._0.ToolsXML.Services.BuildXML.CFDI_4_0.ComplementosPago_40
{   
    public static class InitObj
    {
        // FUNCIONES PARA INICIALIZAR EL OBJETO COMPROBANTE - - - - - - - - - - - - - - - - - - - - - - - - 
        public static Comprobante IniciarObjComprobanteP(decimal TotalPagos, decimal SubTotalPagos, decimal ImporteIvaPagos)
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
                        TotalTrasladosImpuestoIVA16Specified = true,

                        MontoTotalPagos = Math.Round(TotalPagos, 2),
                        TotalTrasladosBaseIVA16 = Math.Round(SubTotalPagos, 2),
                        TotalTrasladosImpuestoIVA16 = Math.Round(ImporteIvaPagos, 2)
                    }
                    //Pago = new PagosPago[] { }
                }
                // - - - - - - - - - - - - - - - - - - - - - - - -
            };

            return oComprobante;
        }   

        public static PagosPago CrearPago(string FormaDePagoP, decimal Total, decimal SubTotal, decimal ImporteIva, string NumOperacion = "")
        {
            return new PagosPago()
            {
                FechaPago = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss"), //PERMITIR PASAR
                FormaDePagoP = FormaDePagoP,
                MonedaP = "MXN",
                TipoCambioP = 1m,
                TipoCambioPSpecified = true,
                Monto = Math.Round(Total, 2),
                NumOperacion = NumOperacion != "" ? NumOperacion : null,
                // - - - - - - - - - - - - - - - - - - - - - - - -
                ImpuestosP = new PagosPagoImpuestosP
                {
                    TrasladosP = new PagosPagoImpuestosPTrasladoP[]
                    {
                        new PagosPagoImpuestosPTrasladoP
                        {
                            BaseP = SubTotal, //EN ESTE APARTADO NO SE REALIZA EL REDONDEO
                            ImpuestoP = "002",
                            TipoFactorP = "Tasa",
                            TasaOCuotaP = 0.160000m,
                            TasaOCuotaPSpecified = true,
                            ImporteP = ImporteIva, //EN ESTE APARTADO NO SE REALIZA EL REDONDEO
                            ImportePSpecified = true
                        }
                    }
                }
            };
        }
                
        public static PagosPagoDoctoRelacionado CrearDocsRelacionados (decimal ImpSaldoAnt, decimal ImpPagado, decimal ImpSaldoInsoluto, decimal ImpPagadoSubTotal, decimal ImporteIva, string UUID, string Serie, string Folio, string parcialidad)
        {
            // - - - - - - - - - - - - - - - - - - - - - - - -
            var DoctoRelacionado = new PagosPagoDoctoRelacionado
            {
                IdDocumento = UUID,
                Serie = Serie,
                Folio = Folio,
                MonedaDR = "MXN",
                NumParcialidad = parcialidad,
                ImpSaldoAnt = Math.Round(ImpSaldoAnt, 2),
                ImpPagado = Math.Round(ImpPagado, 2),
                ImpSaldoInsoluto = Math.Round(ImpSaldoInsoluto, 2),
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
                            BaseDR = ImpPagadoSubTotal,
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
            };

            return DoctoRelacionado;
        }
    }
}
