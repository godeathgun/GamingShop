var product = {
    init: function () {
        product.loadImages();
    },
    loadImages: function () {
        $.ajax({
            url: '/Product/LoadImages',
            type: 'GET',
            data: {
                id: $('#hiProductID').val()
            },
            dataType: 'json',
            success: function (response) {
                var data = response.data;
                var html = '';
                $.each(data, function (i, item) {
                    html += '<div class="details_image_thumbnail" data-image= ' + item + ' >'+
                        '<img src = ' + item + ' alt = "" />' +
                        '</div>'
                });
                $('#imageList').html(html);
            }
        });
    }
}
product.init();