$(document).ready(function () {
    $('#employeeDetail').DataTable({
        serverSide: true,
        processing: true,
        paging: true,
        searching: true,
        bSort: true,
        info: false,
        ajax: {
            url: "/Employee/GetData",
            type: "POST",
            datatype: "json",
            contentType: "application/json; charset=utf-8",
            data: function (data) {
                console.log("Data sent to server:", data);
                console.log("Json Data sent to server:", JSON.stringify(data));
                return JSON.stringify(data);
            }
        }, "columns": [
            { "data": "FirstName", "name": "First Name", "autoWidth": true },
            { "data": "LastName", "name": "Last Name", "autoWidth": true },
            { "data": "EmailAddress", "name": "Email", "autoWidth": true },
            { "data": "CountryName", "name": "Country" },
            { "data": "StateName", "name": "State" },
            { "data": "CityName", "name": "City" },
            { "data": "PanNumber", "name": "Pan No", "autoWidth": true },
            { "data": "PassportNumber", "name": "Passport No", "autoWidth": true },
            { "data": "Gender", "name": "Gender" },
            { "data": "ActiveStatus", "name": "" },
            {
                "data": "ProfileImage", "name": "Profile Image", "autoWidth": true,
                "render": function (data, type, rowImg) {
                    return '<img src="/uploads/' + rowImg.ProfileImage + '" alt = "" width = "40" height = "40" />';
                }
            },
            {
                "data": null, "name": "Action", "autoWidth": true ,
                "render": function (data, type, row) {
                    return `<button class="btn btn-success" onclick="EditEmployee('${row.EmployeeCode}')"><i class="fa-solid fa-pen-to-square"></i></button>
                                                    <button class="btn btn-danger" onclick="SoftDelete('${row.EmployeeCode}')"><i class="fa-solid fa-trash"></i></button>`;
                }
            }
        ]
    });
});

function EditEmployee(employeeCode) {
    window.location.href = 'Employee/EditEmployee?employeeCode=' + employeeCode;
}
function SoftDelete(employeeCode) {
    console.log(employeeCode);
    Swal.fire({
        title: "Do you want to delete the Employee?",
        showCancelButton: true,
        confirmButtonText: "Delete",
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                type: "DELETE",
                url: "Employee/DeleteEmployee",
                data: { employeeCode: employeeCode },
                success: function (data) {
                    if (data.success) {
                        Swal.fire("Deleted!", data.message, "success").then(function () {
                            location.reload();
                        });
                    } else {
                        Swal.fire("Error!", data.message, "error");
                    }
                },
                error: function () {
                    Swal.fire({
                        icon: "error",
                        title: "Oops...",
                        text: "Something went wrong!"
                    });
                }
            });
        }
    });
}
//function SoftDelete(employeeCode) {
//    console.log(employeeCode);
//    Swal.fire({
//        title: "Do you want to delete the Employee?",
//        showCancelButton: true,
//        confirmButtonText: "Delete",
//    }).then((result) => {
//        if (result.isConfirmed) {
//            $.ajax({
//                type: "POST",
//                url: "Employee/DeleteEmployee",
//                data: { employeeCode: employeeCode },
//                success: function (data) {
//                    if (data.success) {
//                        Swal.fire("Deleted!", data.message, "success").then(function () {
//                            location.reload();
//                        });
//                    } else {
//                        Swal.fire("Error!", data.message, "error");
//                    }
//                },
//                error: function () {
//                    Swal.fire({
//                        icon: "error",
//                        title: "Oops...",
//                        text: "Something went wrong!"
//                    });
//                }
//            });
//        }
//    });
//}