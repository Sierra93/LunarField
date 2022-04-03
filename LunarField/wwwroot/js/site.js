var aProfileData = [];

window.onload = function () {
    if (window.location.href.includes("Profile")) {
        this.getProfileData();
        $(".block-avatar").hide();
        document.getElementById('userlogin').value = localStorage["userLogin"];
        document.getElementById('userlogin2').value = localStorage["userLogin"];
        document.getElementById('userlogin3').value = localStorage["userLogin"];
        document.getElementById('userlogin4').value = localStorage["userLogin"];
        document.body.style.backgroundColor = localStorage["profileColor"];
    }
}

function onCreateUser() {
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

function getProfileData() {
    return $.ajax({
        url: '/User/GetProfileData?userName=test',
        type: "GET",
        dataType: "json",

        success: (response) => {
            console.log("Данные профиля", response);
            this.aProfileData = response;
            console.log("aProfileData", this.aProfileData);
            localStorage["profileColor"] = response[0].profileColor;
            
        },

        error: (XMLHttpRequest, textStatus, errorThrown) => {
            console.log("Ошибка получения профиля");
        }
    });
}

function onChangeAvatar() {
    $(".block-avatar").show();
}

// function onSaveProfileData() {
//     let data2 = new FormData();
//     data2.append("files", document.getElementById("idAvatarFile").files[0]);
//
//     return $.ajax({
//         url: '/User/SaveProfileData',
//         type        : 'POST',
//         contentType : 'multipart/form-data',
//         data: {data2},
//
//         success: (response) => {
//             console.log("Успешное сорханение", response);
//         },
//
//         error: (XMLHttpRequest, textStatus, errorThrown) => {
//             console.log("Ошибка соранения профиля");
//         }
//     });
// }

function onSaveProfileColor() {
    return $.ajax({
        url: '/User/SaveProfileColor',
        type: "POST",
        dataType: "json",
        data: {
            UserLogin: document.getElementById("userlogin4").value,
            ProfileColor: document.getElementById("color").value
        },

        success: (response) => {
            window.location.reload();
        },

        error: (XMLHttpRequest, textStatus, errorThrown) => {
            window.location.reload();
            this.getProfileData();
        }
    });
}

function onRouteSignUp() {
    window.location.href = "https://localhost:44379/User/SignUp";
}

function onRouteSignIn() {
    window.location.href = "https://localhost:44379/User/SignIn";
}

function onSignIn() {
    return $.ajax({
        url: '/User/SignIn',
        type: "POST",
        dataType: "json",
        data: {
            UserLogin: document.getElementById("idUserLogin").value,
            UserPassword: document.getElementById("idUserPassword").value
        },

        success: (response) => {
            if(response.userLogin !== "") {
                localStorage["userLogin"] = response.userLogin;
                window.location.href = "https://localhost:44379/User/Profile";
            }
        },

        error: (XMLHttpRequest, textStatus, errorThrown) => {
            console.log("Ошибка ");
        }
    });
}

function onRouteStart() {
    window.location.href = "https://localhost:44379";
}

function onExit() {
    localStorage.clear();
    window.location.href = "https://localhost:44379";
}