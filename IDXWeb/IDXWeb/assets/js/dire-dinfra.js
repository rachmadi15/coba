$(function () {
    if ($('.dire-dinfra-tab-js').length > 0) {
        var $tabEtfDireDinfra = $('.dire-dinfra-tab-js');
        var $etfContent = $('.dire-dinfra-container');

        $tabEtfDireDinfra.on('click', function (event) {
            event.preventDefault();
            /* Act on the event */
            $etfContent.removeClass('is-active');
            $tabEtfDireDinfra.removeClass('is-active');
            var otherElem = $(this).attr('href');
            $(otherElem).addClass('is-active');
            $(this).addClass('is-active');
        });
    }
});