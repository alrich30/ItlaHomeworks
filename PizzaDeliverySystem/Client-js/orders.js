// orders.js

const statusDiv = document.getElementById('connectionStatus');
const contentDiv = document.getElementById('orderContent');

// Función para mostrar estado
function showStatus(message, type) {
    statusDiv.innerHTML = `<div class="status ${type}">${message}</div>`;
}

// ==================== READ - Listar Órdenes ====================

async function loadOrders() {
    showStatus('⏳ Cargando órdenes...', 'loading');
    contentDiv.innerHTML = '';
    
    try {
        const orders = await fetchOrders();
        
        if (orders.length === 0) {
            contentDiv.innerHTML = '<p style="text-align: center; padding: 40px; color: #666;">No hay órdenes en el sistema.</p>';
            showStatus('ℹ️ No se encontraron órdenes.', 'success');
            return;
        }
        
        showStatus(`✅ Se cargaron ${orders.length} órdenes correctamente.`, 'success');
        
        displayOrders(orders);
        
    } catch (error) {
        showStatus(`❌ Error al cargar órdenes: ${error.message}`, 'error');
        contentDiv.innerHTML = '<p style="text-align: center; padding: 40px; color: #dc3545;">Error al cargar las órdenes.</p>';
    }
}

// Mostrar órdenes en la tabla
function displayOrders(orders) {
    contentDiv.innerHTML = `
        <div class="order-table">
            <table>
                <thead>
                    <tr>
                        <th>ID</th>
                        <th>Cliente</th>
                        <th>Fecha</th>
                        <th>Total</th>
                        <th>Estado</th>
                        <th style="text-align: center;">Acciones</th>
                    </tr>
                </thead>
                <tbody id="ordersTableBody">
                </tbody>
            </table>
        </div>
    `;
    
    const tbody = document.getElementById('ordersTableBody');
    
    orders.forEach(order => {
        const row = document.createElement('tr');
        
        // Obtener clase CSS para el estado
        const statusClass = getStatusClass(order.status);
        const statusIcon = getStatusIcon(order.status);
        const statusText = getStatusText(order.status);
        
        row.innerHTML = `
            <td><strong>#${order.id.substring(0, 8)}</strong></td>
            <td>${order.customerId ? order.customerId.substring(0, 8) : 'N/A'}</td>
            <td>13/12/2025</td>
            <td style="font-weight: bold; color: #28a745;">$${order.total.toFixed(2)}</td>
            <td>
                <span class="status-badge ${statusClass}">
                    ${statusIcon} ${statusText}
                </span>
            </td>
            <td style="text-align: center;">
                <button onclick="viewOrderDetail('${order.id}')" 
                        style="background: #007bff; color: white; border: none; padding: 6px 14px; border-radius: 4px; cursor: pointer;">
                    👁️ Ver
                </button>
            </td>
        `;
        
        tbody.appendChild(row);
    });
}

// Helpers para el estado
function getStatusClass(status) {
    const classes = {
        'Created': 'status-created',
        'Confirmed': 'status-confirmed',
        'InKitchen': 'status-inkitchen',
        'OutForDelivery': 'status-outfordelivery',
        'Delivered': 'status-delivered',
        'Cancelled': 'status-cancelled'
    };
    return classes[status] || 'status-created';
}

function getStatusIcon(status) {
    const icons = {
        'Created': '🔵',
        'Confirmed': '🟡',
        'InKitchen': '👨‍🍳',
        'OutForDelivery': '🚚',
        'Delivered': '✅',
        'Cancelled': '❌'
    };
    return icons[status] || '⚪';
}

function getStatusText(status) {
    const texts = {
        'Created': 'Created',
        'Confirmed': 'Confirmed',
        'InKitchen': 'In Kitchen',
        'OutForDelivery': 'Out for Delivery',
        'Delivered': 'Delivered',
        'Cancelled': 'Cancelled'
    };
    return texts[status] || status;
}

// ==================== Ver Detalle de Orden ====================

async function viewOrderDetail(orderId) {
    showStatus('⏳ Cargando detalle de la orden...', 'loading');
    
    try {
        const order = await getOrderById(orderId);
        
        const statusIcon = getStatusIcon(order.status);
        const statusText = getStatusText(order.status);
        const statusClass = getStatusClass(order.status);
        
        contentDiv.innerHTML = `
            <div class="detail-container">
                <div class="detail-header">
                    <h2>📦 Detalle del Pedido #${order.id.substring(0, 8)}</h2>
                    <button onclick="loadOrders()" style="background: #6c757d; color: white; border: none; padding: 8px 16px; border-radius: 4px; cursor: pointer;">
                        ← Volver a la lista
                    </button>
                </div>
                
                <!-- Información del cliente y fecha -->
                <div class="info-grid">
                    <div class="info-section">
                        <h3>👤 Información del Cliente</h3>
                        <p><strong>Email:</strong> juan@email.com</p>
                        <p><strong>ID Cliente:</strong> ${order.customerId ? order.customerId.substring(0, 8) : 'N/A'}</p>
                    </div>
                    <div class="info-section">
                        <h3>📅 Información del Pedido</h3>
                        <p><strong>Fecha:</strong> 13/12/2025 - 14:30</p>
                        <p><strong>Estado:</strong> <span class="status-badge ${statusClass}">${statusIcon} ${statusText}</span></p>
                    </div>
                </div>
                
                <!-- Dirección de entrega -->
                <div class="info-section" style="margin-bottom: 25px;">
                    <h3>📍 Dirección de Entrega</h3>
                    <p><strong>Calle:</strong> ${order.street || 'N/A'}</p>
                    <p><strong>Ciudad:</strong> ${order.city || 'N/A'}</p>
                    <p><strong>Código Postal:</strong> ${order.postalCode || 'N/A'}</p>
                </div>
                
                <!-- Items del pedido -->
                <div>
                    <h3 style="color: #007bff; margin-bottom: 15px;">🍕 Items del Pedido</h3>
                    <table class="items-table">
                        <thead>
                            <tr>
                                <th>Cantidad</th>
                                <th>Pizza</th>
                                <th style="text-align: right;">Precio Unitario</th>
                                <th style="text-align: right;">Subtotal</th>
                            </tr>
                        </thead>
                        <tbody>
                            ${order.items && order.items.length > 0 ? order.items.map(item => `
                                <tr>
                                    <td><strong>${item.quantity}x</strong></td>
                                    <td>${item.pizzaName || 'Pizza'}</td>
                                    <td style="text-align: right;">$${item.unitPrice.toFixed(2)}</td>
                                    <td style="text-align: right; font-weight: bold;">$${item.lineTotal.toFixed(2)}</td>
                                </tr>
                            `).join('') : '<tr><td colspan="4" style="padding: 20px; text-align: center; color: #666;">No hay items</td></tr>'}
                        </tbody>
                        <tfoot>
                            <tr>
                                <td colspan="3" style="text-align: right;">TOTAL:</td>
                                <td style="text-align: right; color: #28a745;">$${order.total.toFixed(2)}</td>
                            </tr>
                        </tfoot>
                    </table>
                </div>
                
                <!-- Botones de acción (Visual - No funcionales aún) -->
                <div class="action-buttons">
                    <button style="padding: 12px 24px; background: #ffc107; color: #333; border: none; border-radius: 6px; cursor: pointer; font-weight: bold;">
                        🔄 Cambiar Estado ▼
                    </button>
                    ${order.status === 'Created' || order.status === 'Confirmed' ? `
                        <button style="padding: 12px 24px; background: #dc3545; color: white; border: none; border-radius: 6px; cursor: pointer; font-weight: bold;">
                            ❌ Cancelar Orden
                        </button>
                    ` : ''}
                    <button onclick="window.print()" style="padding: 12px 24px; background: #6c757d; color: white; border: none; border-radius: 6px; cursor: pointer; font-weight: bold;">
                        🖨️ Imprimir
                    </button>
                </div>
            </div>
        `;
        
        showStatus('', '');
        
    } catch (error) {
        showStatus(`❌ Error al cargar detalle: ${error.message}`, 'error');
    }
}