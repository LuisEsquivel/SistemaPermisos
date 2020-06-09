
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

    var tabla = document.getElementById("table");
    var contenido = "";

    contenido += "<tr>";


    for (i = 0; i < arrayColumnas.length; i++) {
        contenido += "<th>";
        contenido += arrayColumnas[i];
        contenido += "</th>";
    }

    contenido += "<th> Acciones </th>" ;

    contenido += "</tr>";

    var llaves = Object.keys(data[0]);

    for (row = 0; row < data.length; row++) {

        contenido += "<tr>";

        for (celda = 0; celda < arrayColumnas.length; celda++) {

            var cell = arrayColumnas[celda];

            contenido += "<td>";
            contenido += data[row][cell];
            contenido += "</td>";
        }

       
     
        var id = llaves[0];
      
        contenido += "<td>";
        contenido += "<button id='BtnEditar' class='editar btn btn-info btn-sm pb-5 style='height:30px' onclick='RecuperarInfo(" +id+"); '>  <i class='fa fa-pencil-square-o' aria-hidden='true'></i></button>";
        contenido += "<button class='eliminar btn btn-danger btn-sm pb-5 pl-5' style='height:30px' '> <i class='fa fa-trash' aria-hidden='true'></i></button>";
        contenido += "</td>";


        contenido += "</tr>";

    }



    tabla.innerHTML = contenido;

    var buttonAdd = "<input type='button' id='BtnAdd' value='Add' class='btn btn-success mb-5' onclick='AbrirModal()'/>";
    var ButtonAdd = document.getElementById("ButtonAdd");
    ButtonAdd.innerHTML = buttonAdd;
}




var add = function (urlAdd, urlList, parameters, arrayColumnas) {

    console.log(parameters);

    $.ajax({
        method: "POST",
        url: urlAdd,
        data: parameters, /*parámetros enviados al controlador*/
        processData: false,
        contentType: false,
        dataType: "json",


        success: function (data) {

            if (data == 1) {
                alert(GuardadoCorrectamente);
            } else {
                alert(ErrorAlGuardar);
            }
            listar(urlList, arrayColumnas, data);
            Limpiar();
            CerrarModal();

        },
        error: function () {
            console.log("No se ha podido obtener la información");
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



//function RecuperarInfo() {

//    var valores = "";

//    // vamos al elemento padre (<tr>) y buscamos todos los elementos <td>
//    // que contenga el elemento padre
//    var rowCol = document.getElementById("BtnEditar").parentNode.parentNode;

//    //// recorremos cada uno de los elementos del array de elementos <td>
//    //for (let i = 0; i < rowCol.length; i++) {

//    //    // obtenemos cada uno de los valores y los ponemos en la variable "valores"
//    //    valores += elementosTD[i].innerHTML + "\n";
//    //}

//    //alert(valores);

    
//    console.log(rowCol);
//}



var filter = function (url, parameters) {

    $.ajax({
        method: "GET",
        url: url,
        data: parameters, /*parámetros enviados al controlador*/
        processData: false,
        contentType: false,
        dataType: "json",

        success: function (data) {

            if (data != null) {
                LlenarModal(data);
            }
            
        },
        error: function () {
            console.log("No se ha podido obtener la información");
        }
    });

}


function LlenarModal(data) {

    $("#ID").val(data.ID);
    $("#NOMBRE").val(data.NOMBRE);
}