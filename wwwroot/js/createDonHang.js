function validateInput() {
    const input = document.getElementById("SoLuongText").value.trim();
    const errorDiv = document.getElementById("errorText");

    const regex = /^(\d+(,\d+)?)( \+ \d+(,\d+)?)*$/;

    if (!regex.test(input)) {
        errorDiv.innerText = "Định dạng không hợp lệ! Hãy nhập đúng như: 1,1 + 2 + 3,5";
        return false;
    }

    errorDiv.innerText = "";
    return true;
}
