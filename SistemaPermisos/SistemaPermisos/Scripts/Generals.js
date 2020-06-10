
var ErrorAlGuardar = "Ocurrió un error al guardar";
var GuardadoCorrectamente = "Se guardó correctamente";



var listar = function (url, arrayColumnas, parameters) {

    $.ajax({
        method: "GET",
        url: url,
        data: parameters, /*parámetros enviados al controlador*/
        contentType: "application/json;charset=utf-8",
        dataType: "json",

        success: function (data) {

                window.Table(arrayColumnas, data);
  

        },
        error: function () {
            console.log("No se ha podido obtener la información");
        }
    });

}







function Table(arrayColumnas, data) {

    var container = document.getElementById("container");;
    container.classList.add("container");
    var contenido = "";

    contenido += "<input type='button' id='BtnAdd' value='Add' class='btn btn-success' onclick='AbrirModal()'/>";

    contenido += "<table class='table mt-5'>";

    contenido += "<tr>";

    for (i = 0; i < arrayColumnas.length; i++) {
        contenido += "<th>";
        contenido += arrayColumnas[i];
        contenido += "</th>";
    }

    contenido += "<th> Acciones </th>" ;

    contenido += "</tr>";

 
  
    var id;

    for (row = 0; row < data.length; row++) {

        contenido += "<tr>";

        for (celda = 0; celda < arrayColumnas.length; celda++) {

            var cell = arrayColumnas[celda];

            contenido += "<td>";
            contenido += data[row][cell];
            contenido += "</td>";


            /*DE MANERA PERRONA OBTENEMOS EL ID PARA FILTRAR*/
            if (celda == 0) {
                id = data[row][cell];
            }
       
        }



 
        contenido += "<td>";
        contenido += "<button id='BtnEditar' class='editar btn btn-info btn-sm pb-5 style='height:30px' onclick='RecuperarInfo("+id+");'>  <i class='fa fa-pencil-square-o' aria-hidden='true'></i></button>";
        contenido += "<button class='eliminar btn btn-danger btn-sm pb-5 pr-5' style='height:30px' '> <i class='fa fa-trash' aria-hidden='true'></i></button>";
        contenido += "</td>";


        contenido += "</tr>";

    }

    contenido += "</table>";

    container.innerHTML = contenido;


}




var add = function (urlAdd, urlList, parameters, arrayColumnas) {

    var datos;

    $.ajax({
        method: "POST",
        url: urlAdd,
        data: parameters, /*parámetros enviados al controlador*/
        processData: false,
        contentType: false,
        dataType: "json",

        success: function (data) {
            datos = data;

            if (data == 1) {
                alert(GuardadoCorrectamente);
            } else {
                alert(ErrorAlGuardar);
            }
            listar(urlList, arrayColumnas, data);
            Limpiar();
            CerrarModal();

           
        },
        error: function (xhr, status, error) {
            var err = JSON.parse(xhr.responseText);
            alert(err.Message);
        }
    });

}



function AbrirModal() {
    $("#modal").modal("show");
}

function CerrarModal() {
    $("#modal").modal("hide");
}

function Limpiar() {
    $('#form').trigger("reset");
}



var filter = function (url, parameters, llenarModal , llenarTable, campos) {

    $.ajax({
        method: "POST",
        url: url,
        data: parameters, /*parámetros enviados al controlador*/
        processData: false,
        contentType: false,
        dataType: "json",


        success: function (data) {

            if (data != null && llenarModal == true) {
                LlenarModal(data, campos);
                AbrirModal();
            }

            if (data != null && llenarTable == true) {
                Table(arrayColumnas, data);
            }
        },
        error: function () {
            console.log("No se ha podido obtener la información");
        }
    });

}


function LlenarModal(data, campos) {

    //obtenemos los keys del JSON ejemplo: ID, NOMBRE...
    var keys = Object.keys(data[0]);

    for (row = 0; row < data.length; row++) {

        //recorremos los campos que vamos a pintar en el modal
        for (columna = 0; columna < campos.length; columna++) {

            //recorremos las keys
            for (o = 0; o < keys.length; o++) {

                if (keys[o] == campos[columna]) {

                    //celda actual
                    var celda = campos[columna];

                    //obtenemos el valor de la celda
                    var value = data[row][celda];

                    //agregamos el valor obtenido al control mediante jquery el nombre del ID debe ser igual que la celda
                    $("#"+celda).val(value);
                 
                }

            }

        }

    }

}