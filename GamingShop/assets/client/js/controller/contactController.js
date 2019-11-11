var contact = {
    init: function () {
        contact.registerEvents();
    },
    registerEvents: function () {
        $('#btnSend').off('click').on('click', function () {
            var name = $('txtName').val();
            var email = $('txtEmail').val();
            var content = $('txtContent').val();

            $ajax({
                url: 'Contact/Send',
                type: 'POST',

            })
        }); 
    }
}
contact.init();