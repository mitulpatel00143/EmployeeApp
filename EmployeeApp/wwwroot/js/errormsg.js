
var errorMessage = '@TempData["ErrorMessage"]';

if (errorMessage) {
    Swal.fire({
        icon: 'error',
        title: 'Error',
        text: errorMessage,
    });
}