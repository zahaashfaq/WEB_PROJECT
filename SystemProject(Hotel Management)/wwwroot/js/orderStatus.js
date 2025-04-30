const connection = new signalR.HubConnectionBuilder().withUrl("/orderHub").build();

connection.start()

connection.on("ReceiveOrderStatusUpdate", (orderId, status) => {
   
    let statusElement = document.getElementById(`order-status-${orderId}`);
    if (statusElement) {
        statusElement.textContent = status;
    }
});


function updateOrderStatus(orderId) {
    let dropdown = document.getElementById(`status-dropdown-${orderId}`);
    let newStatus = dropdown.value;

    fetch(`/Cart/UpdateOrderStatus`, {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify({ orderId, status: newStatus })
    })
        .then(response => response.json())
        .then(data => {
            if (data.success) {
                console.log(`Order ${orderId} updated to ${newStatus}`);
            } else {
                console.error("Failed to update order status.");
            }
        })
        .catch(error => console.error("Error:", error));
}
