function ShowToast(providedText, durationMs) {
    Toastify({
        text: providedText,
        duration: durationMs,
        newWindow: true,
        close: false,
        gravity: "top", // `top` or `bottom`
        position: "center", // `left`, `center` or `right`
        stopOnFocus: true, // Prevents dismissing of toast on hover
        style: {
            background: "linear-gradient(to right, #FF4500, #8B0000)",
        }
    }).showToast();
}

$(document).ready(function () {
    $("form[name='Register']").validate({
        rules: {
            Username: {
                required: true,
                minlength: 1,
                maxlength: 20
            },
            Email: {
                required: true,
                email: true,
                minlength: 4,
                maxlength: 30
            },
            Password: "required",
        },
        messages: {
            Username: {
                required: "Please enter a username",
                minlength: "Please enter a username",
                maxlength: "Username cannot exceed 20 characters in length"
            },
            Email: {
                required: "Please provide a valid email address",
                minlength: "Please enter a valid email address",
                maxlength: "Email cannot exceed 30 characters in length"
            },
            Password: "Please provide a valid password"
        }
    });
});
