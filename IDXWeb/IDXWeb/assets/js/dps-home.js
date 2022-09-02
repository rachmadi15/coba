$(function () {
    var _extends = function (target) {
        for (var i = 0; i < arguments.length; i++) {
            var source = arguments[i];
            for (var key in source) {
                if (typeof source[key] === 'function') {
                    source[key]();
                }
            }
        }
    };
	function exist(selector) {
        return new Promise(function (resolve, reject) {
            if ($(selector).length > 0) {
                resolve(selector);
            }
            reject();
            // reject('No element found for ' + selector);
        });
    }
	function consoleLog(e) {
        if (typeof e === 'undefined') return;
        console.log(e);
    }
	var accordion = {
        bodAccordion: function bodAccordion() {
            exist('.hof-accordion-js').then(function (res) {
                var $hofContainer = $(res);
                var $showMore = $('.hof-more');
                var $close = $('.hof-close');

                var moreText = $('[data-more]').attr('data-more');
                var closeText = $('[data-more]').attr('data-close');

                $('.hof-figcaption-description').each(function (index, el) {
                    if ($(this).height() > 108) {
                        $(this).addClass('hof-figcaption-description--limit');
                        $(this).parent().append('<a href="" class="hof-more is-active">' + moreText + '</a><a href="" class="hof-close">' + closeText + '</a>');
                    }
                });

                $('body').on('click', '.hof-more', function (event) {
                    event.preventDefault();
                    /* Act on the event */
                    $(this).removeClass('is-active');
                    $(this).parent().find('.hof-figcaption-description').addClass('is-active');
                    $(this).parent().find('.hof-close').addClass('is-active');
                    $(this).parents('.hof-figure').addClass('is-active');
                    $(this).parents('.hof-accordion-js').addClass('main-content');
                });

                $('body').on('click', '.hof-close', function (event) {
                    event.preventDefault();
                    /* Act on the event */
                    $(this).removeClass('is-active');
                    $(this).parent().find('.hof-figcaption-description').removeClass('is-active');
                    $(this).parents('.hof-figure').removeClass('is-active');
                    $(this).parent().find('.hof-more').addClass('is-active');
                    $(this).parents('.hof-accordion-js').removeClass('main-content');
                });
            }).catch(consoleLog);
        },
        faqAccordion: function faqAccordion() {
            exist('.accordion-text').then(function (res) {
                var $faqContainer = $(res);

                $faqContainer.on('click', function (event) {
                    event.preventDefault();
                    /* Act on the event */
                    $(this).toggleClass('is-active');
                    $(this).next().toggleClass('is-active');
                });
            }).catch(consoleLog);
        },
        statisticAccordion: function statisticAccordion() {
            exist('.js-statistic-accordion').then(function (res) {
                var $accordion = $(res);

                $accordion.on('click', '.accordion-head', function (e) {
                    var $this = $(e.currentTarget);
                    var $thisTarget = $($this.data('collapse'));

                    $thisTarget.siblings().removeClass('is-active');

                    $thisTarget.toggleClass('is-active');
                });
            }).catch(consoleLog);
        }
    };

    var filterDisplay = {
        otcTrade: function otcTrade() {
            exist('.otc-radio-js').then(function (res) {
                var $radio = $(res);
                var $otcContainer = $('.otc-filter-container');

                $radio.on('change', function (event) {
                    event.preventDefault();
                    /* Act on the event */
                    $otcContainer.removeClass('is-active');
                    var filterElem = $(this).val();
                    $('#' + filterElem).addClass('is-active');
                });
            }).catch(consoleLog);
        }
    };
	_extends(accordion);
});