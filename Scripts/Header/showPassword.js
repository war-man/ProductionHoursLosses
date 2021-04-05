function showPassword(documentElementId) {
    var x = document.getElementById(documentElementId);
    if (x.type === "password") {
        x.type = "text";
    } else {
        x.type = "password";
    }
}