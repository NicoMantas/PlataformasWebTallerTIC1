import api from './api';
import { jsPDF } from 'jspdf';

export async function listFacturas() {
  const { data } = await api.get('/Factura');
  return data;
}

export async function getFacturaById(id) {
  const { data } = await api.get(`/Factura/${id}`);
  return data;
}

export async function getFacturaByOrden(ordenId) {
  const { data } = await api.get(`/Factura/orden/${ordenId}`);
  return data;
}

export async function getFacturaByServicio(servicioId) {
  console.log('Buscando factura para servicio:', servicioId); // Debug
  try {
    const { data } = await api.get(`/Factura/servicio/${servicioId}`);
    console.log('Respuesta del endpoint:', data); // Debug
    return data;
  } catch (error) {
    console.error('Error obteniendo factura:', error); // Debug
    throw error;
  }
}

export async function createFactura(facturaData) {
  const { data } = await api.post('/Factura', facturaData);
  return data;
}

export async function updateFactura(id, facturaData) {
  const { data } = await api.put(`/Factura/${id}`, facturaData);
  return data;
}

export async function deleteFactura(id) {
  await api.delete(`/Factura/${id}`);
  return true;
}

export async function descargarFacturaPdf(factura) {
  console.log('Iniciando generación de PDF para factura:', factura); // Debug
  
  try {
    const doc = new jsPDF({ unit: 'pt', format: 'a4' });

    const primary = '#0ea5e9';
    const textMuted = '#6b7280';
    const pageWidth = doc.internal.pageSize.getWidth();

  // Header band
  doc.setFillColor(primary);
  doc.rect(0, 0, pageWidth, 80, 'F');
  doc.setTextColor('#ffffff');
  doc.setFontSize(20);
  doc.text('Factura de Servicio', 40, 50);

  // Card container
  const cardX = 40;
  const cardY = 110;
  const cardW = pageWidth - 80;
  const cardH = 400;
  doc.setDrawColor('#e5e7eb');
  doc.setFillColor('#ffffff');
  doc.roundedRect(cardX, cardY, cardW, cardH, 8, 8, 'FD');

  // Title
  doc.setTextColor('#111827');
  doc.setFontSize(16);
  doc.text(`Factura #${factura.id ?? ''}`, cardX + 16, cardY + 28);
  doc.setFontSize(11);
  doc.setTextColor(textMuted);
  doc.text(`Fecha: ${new Date().toLocaleString()}`, cardX + 16, cardY + 46);

  // Info grid
  const label = (x, y, t) => { doc.setTextColor(textMuted); doc.text(t, x, y); };
  const value = (x, y, t) => { doc.setTextColor('#111827'); doc.text(String(t ?? ''), x, y); };

  let y = cardY + 80;
  label(cardX + 16, y, 'Cliente');
  value(cardX + 180, y, factura.clienteNombre ?? factura.ClienteNombre ?? 'N/D');
  y += 20;
  label(cardX + 16, y, 'Vehículo');
  value(cardX + 180, y, factura.vehiculoPlaca ?? factura.VehiculoPlaca ?? 'N/D');
  y += 30;

  // Desglose de costos
  doc.setTextColor('#111827');
  doc.setFontSize(12);
  doc.text('Desglose de Costos', cardX + 16, y);
  doc.setFontSize(11);
  doc.setTextColor(textMuted);
  y += 20;

  const subtotal = factura.subtotal ?? factura.Subtotal ?? 0;
  const impuestos = factura.impuestos ?? factura.Impuestos ?? 0;
  const total = factura.total ?? factura.Total ?? 0;

  label(cardX + 16, y, 'Subtotal');
  value(cardX + 180, y, `$${subtotal.toLocaleString()}`);
  y += 20;
  
  label(cardX + 16, y, 'IVA (19%)');
  value(cardX + 180, y, `$${impuestos.toLocaleString()}`);
  y += 20;
  
  doc.setFontSize(12);
  doc.setTextColor('#111827');
  label(cardX + 16, y, 'Total');
  value(cardX + 180, y, `$${total.toLocaleString()}`);

  // Divider
  y += 24;
  doc.setDrawColor('#e5e7eb');
  doc.line(cardX + 16, y, cardX + cardW - 16, y);
  y += 24;

  // Services
  doc.setTextColor('#111827');
  doc.setFontSize(12);
  doc.text('Servicios Realizados', cardX + 16, y);
  doc.setFontSize(11);
  doc.setTextColor(textMuted);
  y += 18;

  const servicios = factura.serviciosRealizados ?? factura.ServiciosRealizados ?? [];
  if (servicios && servicios.length > 0) {
    servicios.forEach(servicio => {
      doc.text(`• ${servicio}`, cardX + 16, y);
      y += 16;
    });
  } else {
    doc.text('No hay servicios registrados', cardX + 16, y);
  }

  // Footer
  y = cardY + cardH - 40;
  doc.setDrawColor('#e5e7eb');
  doc.line(cardX + 16, y, cardX + cardW - 16, y);
  y += 20;
  doc.setTextColor(textMuted);
  doc.setFontSize(10);
  doc.text('Gracias por confiar en nuestros servicios', cardX + 16, y);

  console.log('PDF generado exitosamente'); // Debug
  return doc.output('blob');
  
  } catch (error) {
    console.error('Error generando PDF:', error); // Debug
    throw new Error(`Error al generar PDF: ${error.message}`);
  }
}
