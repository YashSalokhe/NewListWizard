// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

showInPopup = (url, title) => {

    var holder = $('#placeHolder');
    $.ajax({
        type: "GET",
        url: url,
        success: function (res) {
            holder.html(res);
            //$("#form-modal .modal-title").html(title);
            holder.find('.modal .modal-title').html(title);
            holder.find('.modal').modal('show');

        }
    })
}


function submitFormData() {

    var radioButtonvalue;

    if ($('#iscsv').is(':checked')) {
        radioButtonvalue = 1;
        var wizardList = {
            ListId: 0,
            ListName: $('#listname').val(),
            AssignedTo: $('#assignedto').val(),
            CreatedDate: /*$('#createdate').val()*/ null,
            IsCsv: radioButtonvalue,
            ModifiedDate: null,
            IsDeleted: 0,
            CsvContents: {}
        }
        console.log(wizardList);
        // console.log(wizardList);

        $.ajax({
            type: "POST",
            url: "/Wizard/CreateNewListPartial",
            data: { 'wizardList': wizardList },
            success: function () {
                showInPopup("/Wizard/FileUploadPartial", 'Upload')
            }
        });
    }
    else {
       // radioButtonvalue = 0;
        alert("Please tick csv button");
    }
    
}

$(document).ready(function () {
    $('#uploadfile').change(function () {

        var files = $('#uploadfile')[0].files;
        var fname = files[0].name;
        console.log(files[0].name);
        var ext = fname.substring(fname.length - 3, fname.length).toString();
        console.log(ext);
        extext = ext.toLowerCase();
        if (extext == 'csv') {
            formData = new FormData();
            formData.append("uploadedFile", files[0]);


            $.ajax({
                type: "POST",
                url: "/Wizard/FileUploadPartial",
                cache: false,
                contentType: false,
                processData: false,
                data: formData,
                success: function (data) {
                    console.log(data.missing)
                    $('#showContent').append($('<div>')).load("/Wizard/Display")
                }
            })
        } else {
            alert("please upload csv file");
        }
       

    });
});

function backToWizard() {
    debugger;
    $.ajax({
        type: "GET",
        url: "/Wizard/CreateNewListPartial",
        success: function () {
            showInPopup("/Wizard/CreateNewListPartial", 'New List')
        }
    });
    //var holder = $('#placeHolder');
    //$.ajax({
    //    type: "GET",
    //    url: url,
    //    success: function (res) {
    //        holder.html(res);
    //        //$("#form-modal .modal-title").html(title);
    //        holder.find('.modal .modal-title').html(title);
    //        holder.find('.modal').modal('show');

    //    }
    //})
}

//function onUploadSubmit() {

//   /* $("#fileUpload").change(function () {*/
//       // var files = $('#fileUpload').prop("files");
//        var files = $('#uploadfile')[0].files;
//        formData = new FormData();
//        formData.append("uploadedFile", files[0]);


    

//        debugger;
//        $.ajax({
//            type: "POST",
//            url: "/Wizard/FileUploadPartial",
//            cache: false,
//            contentType: false,
//            processData: false,
//            data: formData,
//            success: function (data) {
//                console.log(data)
//            }
//        })
//   /* })*/
//}

//function onClose() {
//    $.ajax({
//        type: "GET",
//        url: "/Wizard/Index"
//    });
//}

$(document).ready(function () {
    $('#gridView').DataTable();
});

//{
//    pageLength: 2,
//        ajax: {
//        url: "/Wizard/Index",
//            dataSrc: ''
//    },
//    columns: [
//        {
//            title: 'ListName',
//            data: 'ListName'
//        },
//        {
//            title: 'AssignedTo',
//            data: 'AssignedTo'
//        },
//        {
//            title: 'CreatedDate',
//            data: 'CreatedDate'
//        },
//        {
//            title: 'ModifiedDate',
//            data: 'ModifiedDate'
//        }

//    ]
//}

$(document).ready(function () {
    $('#checkBoxAll').click(function () {
        if ($(this).is(":checked"))
            $('.checkBoxId').prop('checked', true);
        else
            $('.checkBoxId').prop('checked', false);
    })
});