﻿var product = {
    init: function () {
        product.registerEvents();
    },
    registerEvents: function () {
        $('.btn-active').off('click').on('click', function (e) {
            e.preventDefault();
            var btn = $(this);
            var id = btn.data('id');
            $.ajax({
                url: "/Admin/Product/ChangeStatus",
                data: { id: id },
                dataType: "json",
                type: "POST",
                success: function (response) {
                    console.log(response);
                    if (response.status == true) {
                        btn.text('Kích hoạt');
                    }
                    else {
                        btn.text('Khoá');
                    }
                }
            });
        });

        $('.btn-images').off('click').on('click', function (e) {
            e.preventDefault();
            $('#imageManage').modal('show');
            $('#hiProductID').val($(this).data('id'));
            product.loadImages();
        });

        $('#btnChooseImage').off('click').on('click', function (e) {
            e.preventDefault();
            var finder = new CKFinder();
            finder.selectActionFunction = function (url) {
                $('#imageList').append('<div style="float:left">' +
                    '<img src=' + url + ' width="100"  />' +
                    '<a href="#" class="btn-removeImage"><i class="fa fa-times"></i></a>' +
                    '</div>');
                $('.btn-removeImage').off('click').on('click', function (e) {
                    e.preventDefault();
                    $(this).parent().remove();
                });
            }
            finder.popup();
        });

        $('#btnSaveImage').off('click').on('click', function () {
            var images = [];
            var id = $('#hiProductID').val();
            $.each($('#imageList img'), function (i, item) {
                images.push($(item).prop('src'));
            });
            console.log(id);
            $.ajax({
                url: '/Admin/Product/SaveImages',
                type: 'POST',
                data: {
                    id: id,
                    images: JSON.stringify(images)
                },
                dataType: 'json',
                success: function (response) {
                    if (response.status) {
                        $('#imageManage').modal('hide');
                        $('#imageList').html('');
                        alert("SUCCESS");
                    }
                }
            });
        });
    },
    loadImages: function () {
        $.ajax({
            url: '/Admin/Product/LoadImages',
            type: 'GET',
            data: {
                id: $('#hiProductID').val()
            },
            dataType: 'json',
            success: function (response) {
                var data = response.data;
                var html = '';
                $.each(data, function (i, item) {
                    html += '<div style="float:left">' +
                        '<img src=' + item + ' width="100"  />' +
                        '<a href="#" class="btn-removeImage"><i class="fa fa-times"></i></a>' +
                        '</div>'
                });
                $('#imageList').html(html);
                product.registerEvents();
                $('.btn-removeImage').off('click').on('click', function (e) {
                    e.preventDefault();
                    $(this).parent().remove();
                });
            }
        });
    }
}
product.init();