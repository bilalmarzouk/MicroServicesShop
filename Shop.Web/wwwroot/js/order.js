var dataTable;
$(document).ready(function () {
    var url = window.location.search;
    if (url.includes("approved")) {
        loadDataTable("approved");
    }
    else if (url.includes("readyforpickup")) {
        loadDataTable("readyforpickup");
    }
    else if (url.includes("cancelled")) {
        loadDataTable("cancelled");
    }
    else {
        loadDataTable("all");
    }
});
/*https://datatables.net/*/
function loadDataTable(status) {
    try {
        dataTable = $('#tblData').DataTable({
            order: [0, 'desc'],
            "ajax": { url: "/order/getall?status=" + status, "type": "GET", "datatype": "json" },
            "columns": [
                { "data": "orderHeaderId", "width": "5%" },
                { "data": "email", "width": "25%" },
                { "data": "firstName", "width": "20%" },
                { "data": "lastName", "width": "20%" },
                { "data": "phone", "width": "20%" },
                { "data": "status", "width": "20%" },
                { "data": "orderTotal", "width": "15%" },
                {
                    "data": "orderHeaderId", "render": function (data) {
                        return `<div class="w-75 btn-group" role="group">
                         <a href="/order/orderDetail?orderId=${data}" class ="btn btn-primary mx-2"><i class="bi bi-pencil-square"></i></a>
                         </div>`
                    }, "width": "10%"
                }
            ]
        });
    }
    catch (error) {
        console.error(error);
        // Expected output: ReferenceError: nonExistentFunction is not defined
        // (Note: the exact output may be browser-dependent)
    }
}