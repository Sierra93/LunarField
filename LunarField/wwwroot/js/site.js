function createUser() {
    console.log("createUser");

    // var userInput = {
    //     UserLogin: document.getElementById("idUserLogin").value,
    //     FullName: document.getElementById("idFullName").value,
    //     DateYear: document.getElementById("idDateYear").value,
    //     UserPassword: document.getElementById("idUserPassword").value
    // };

    return $.ajax({
        url: '/User/CreateUser',
        type: "POST",
        dataType: "json",
        data: {
            UserLogin: document.getElementById("idUserLogin").value,
            FullName: document.getElementById("idFullName").value,
            DateYear: document.getElementById("idDateYear").value,
            UserPassword: document.getElementById("idUserPassword").value
        },
        success: (response) => {
            alert("Регистрация прошла успешно!");
        },
        
        error: (XMLHttpRequest, textStatus, errorThrown) => {
            console.log("Ошибка регистрации");
        }
    });
}