

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