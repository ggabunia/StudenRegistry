// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(document).ready(function () {
    $('.deleteBtn').click(function () {
        
        

        if (confirm("ნამდვილად გსურთ ამ სტუდენტის წაშლა?")) {
            studentId = $(this).data('id');
            var form = $('#AntiForgeryForm');
            var token = $('input[name="__RequestVerificationToken"]', form).val();

            $.post("home/delete", { id: studentId, __RequestVerificationToken: token }, function (data) {
                alert(data);
                location.reload();
            });
        }

    });
});

$(document).ready(function () {
    $('.datepicker').datepicker({
        format: 'yyyy-mm-dd',
        autoclose: true,
        clearBtn: true,
        enableOnReadonly: true,
        startDate: '-100y',
        endDate: '-16y',
        weekStart: '1',
    });
});