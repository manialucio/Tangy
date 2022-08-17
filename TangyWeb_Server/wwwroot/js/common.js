
window.SweetAlert2 = (type, message) => {

    if (type === 'error') {
        Swal.fire({
            icon: type,
            title: 'Oops...',
            text: 'Something went wrong!',
            footer: '<a href="">Why do I have this issue?</a>'
        })
    }
    else if (type === 'success') {
        Swal.fire({
            position: 'top-end',
            icon: type,
            title: message,
            showConfirmButton: false,
            timer: 1500
        })
    }


}

function ShowDeleteConfirmationModal() {
    $('#deleteConfirmationModal').modal('show');
}
function HideDeleteConfirmationModal() {
    $('#deleteConfirmationModal').modal('hide');
}


window.ShowToastr = (type, message) => {
    if (type === 'success') {
        toastr.success(message, 'ok');
    }
    else if (type === 'error') {
        toastr.error(message, 'ko')
    }
}

