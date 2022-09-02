var Site = (function () {
    'use strict';

    function _toConsumableArray(arr) { if (Array.isArray(arr)) { for (var i = 0, arr2 = Array(arr.length); i < arr.length; i++) { arr2[i] = arr[i]; } return arr2; } else { return Array.from(arr); } }

    function noop() { }







































    function queryAll(context) {
        return function (selector) {
            return [].concat(_toConsumableArray(context.querySelectorAll(selector)));
        };
    }











    function exist(selector) {
        return new Promise(function (resolve, reject) {
            var elems = queryAll(document)(selector);

            if (elems.length) resolve(elems);
            reject('no element found for ' + selector);
        });
    }

    var activeStateMobile = function () {
        document.addEventListener('touchstart', noop, true);
    };

    var WPViewportFix = function () {
        var isIEMobile = '-ms-user-select' in document.documentElement.style && navigator.userAgent.match(/IEMobile/);

        if (!isIEMobile) return;

        var style = document.createElement('style');
        var fix = document.createTextNode('@-ms-viewport{width:auto!important}');
        style.appendChild(fix);
        document.head.appendChild(style);
    };

    var objectFitPolyfill = function () {
        objectFitImages();
    };

    var tabMenu = {
        tabStockInfo: function tabStockInfo() {
            exist('.tab-anchor-js').then(function (res) {
                var $tabAnchor = $(res);
                var $tableContent = $('.table-information-content');

                $tabAnchor.on('click', function (event) {
                    event.preventDefault();
                    event.stopPropagation();
                    /* Act on the event */
                    $tabAnchor.removeClass('is-active');
                    $tableContent.removeClass('is-active');

                    var otherElem = $(this).attr('href');
                    $(otherElem).addClass('is-active');
                    $(this).addClass('is-active');
                });
            }).catch(noop);
        },
        tabMarketOverview: function tabMarketOverview() {
            exist('.tab-market-js').then(function (res) {
                var $tabAnchor = $(res);
                var $chartContent = $('.market-overview-chart-stock');

                $tabAnchor.on('click', function (event) {
                    event.preventDefault();
                    event.stopPropagation();
                    /* Act on the event */
                    $chartContent.removeClass('is-active');
                    $tabAnchor.removeClass('is-active');
                    var otherElem = $(this).attr('href');
                    $(otherElem).addClass('is-active');
                    $(this).addClass('is-active');
                });
            }).catch(noop);
        },
        tabTop5: function tabTop5() {
            exist('.top-5-title-js').then(function (res) {
                var $tabAnchor = $(res);
                var $tableContent = $('.top-5-content-js');

                $tabAnchor.on('click', function (event) {
                    event.preventDefault();
                    /* Act on the event */
                    $tableContent.removeClass('is-active');
                    $tabAnchor.removeClass('is-active');
                    var otherElem = $(this).attr('href');
                    $(otherElem).addClass('is-active');
                    $(this).addClass('is-active');
                });
            }).catch(noop);
        },
        tabPeople: function tabPeople() {
            exist('.people-tab-js').then(function (res) {
                var $tabPeople = $(res);
                var $peopleContent = $('.people-container');
                var moreText = $('[data-more]').attr('data-more');
                var closeText = $('[data-more]').attr('data-close');

                $tabPeople.on('click', function (event) {
                    event.preventDefault();
                    /* Act on the event */
                    $peopleContent.removeClass('is-active');
                    $tabPeople.removeClass('is-active');
                    var otherElem = $(this).attr('href');
                    $(otherElem).addClass('is-active');
                    $(this).addClass('is-active');

                    $('.hof-figcaption-description').each(function (index, el) {
                        if ($(this).height() > 108) {
                            $(this).addClass('hof-figcaption-description--limit');
                            $(this).parent().append('<a href="" class="hof-more is-active">' + moreText + '</a><a href="" class="hof-close">' + closeText + '</a>');
                        }
                    });
                });
            }).catch(noop);
        },
        tabHistory: function tabHistory() {
            exist('.history-tab-js').then(function (res) {
                var $tabHistory = $(res);
                var $historyContent = $('.history-container');

                $tabHistory.on('click', function (event) {
                    event.preventDefault();
                    /* Act on the event */
                    $historyContent.removeClass('is-active');
                    $tabHistory.removeClass('is-active');
                    var otherElem = $(this).attr('href');
                    $(otherElem).addClass('is-active');
                    $(this).addClass('is-active');
                });
            }).catch(noop);
        },
        tabStatistic: function tabStatistic() {
            exist('.statistic-tab-js').then(function (res) {
                var $tabStatistic = $(res);
                var $statisticContent = $('.statistic-container');

                $tabStatistic.on('click', function (event) {
                    event.preventDefault();
                    /* Act on the event */
                    $statisticContent.removeClass('is-active');
                    $tabStatistic.removeClass('is-active');
                    var otherElem = $(this).attr('href');
                    $(otherElem).addClass('is-active');
                    $(this).addClass('is-active');
                });
            }).catch(noop);
        },
        tabEtpTrading: function tabEtpTrading() {
            exist('.etp-trading-tab-js').then(function (res) {
                var $tabEtp = $(res);
                var $etpContent = $('.etp-trading-container');

                $tabEtp.on('click', function (event) {
                    event.preventDefault();
                    /* Act on the event */
                    $etpContent.removeClass('is-active');
                    $tabEtp.removeClass('is-active');
                    var otherElem = $(this).attr('href');
                    $(otherElem).addClass('is-active');
                    $(this).addClass('is-active');
                });
            }).catch(noop);
        },
        exchangeTrade: function exchangeTrade() {
            exist('.exchange-trade-tab-js').then(function (res) {
                var $tabExchange = $(res);
                var $exchangeContent = $('.exchange-trade-container');

                $tabExchange.on('click', function (event) {
                    event.preventDefault();
                    /* Act on the event */
                    $exchangeContent.removeClass('is-active');
                    $tabExchange.removeClass('is-active');
                    var otherElem = $(this).attr('href');
                    $(otherElem).addClass('is-active');
                    $(this).addClass('is-active');
                });
            }).catch(noop);
        },
        tabTradingSummary: function tabTradingSummary() {
            exist('.trading-summary-title-js').then(function (res) {
                var $tabAnchor = $(res);
                var $tableContent = $('.trading-summary-content-js');

                $tabAnchor.on('click', function (event) {
                    event.preventDefault();
                    /* Act on the event */
                    $tableContent.removeClass('is-active');
                    $tabAnchor.removeClass('is-active');
                    var otherElem = $(this).attr('href');
                    $(otherElem).addClass('is-active');
                    $(this).addClass('is-active');
                });
            }).catch(noop);
        },
        companyProfileTab: function companyProfileTab() {
            exist('.company-profile-anchor-js').then(function (res) {
                var $tabAnchor = $(res);
                var $tableContent = $('.company-profile-content');

                $tabAnchor.on('click', function (event) {
                    event.preventDefault();
                    /* Act on the event */
                    $tableContent.removeClass('is-active');
                    $tabAnchor.removeClass('is-active');
                    var otherElem = $(this).attr('href');
                    $(otherElem).addClass('is-active');
                    $(this).addClass('is-active');
                });
            }).catch(noop);
        },
        companyProfileDropdown: function companyProfileDropdown() {
            exist('.financial-report-js').then(function (res) {
                var $dropdown = $(res);
                var $contentContainer = $('.financial-report-content');

                $dropdown.on('change', function (event) {
                    event.preventDefault();
                    /* Act on the event */
                    $contentContainer.removeClass('is-active');
                    var otherElem = $dropdown.val();
                    $(otherElem).addClass('is-active');
                });
            }).catch(noop);
        }
    };

    var navMenu = {
        openMobileNav: function openMobileNav() {
            exist('.open-nav-js').then(function (res) {
                var $openNav = $(res);
                var $mobileNav = $('.header-main-mobile-nav');

                $openNav.on('click', function (event) {
                    event.preventDefault();
                    event.stopPropagation();
                    /* Act on the event */
                    $mobileNav.toggleClass('is-active');
                    $(this).toggleClass('fa-times');
                    $(this).toggleClass('fa-bars');
                });
            }).catch(noop);
        },
        toggleMainNav: function toggleMainNav() {
            exist('.main-nav-js').then(function (res) {
                var $tabMenu = $(res);
                var $navItem = $('.header-main-mobile-nav-item');

                $tabMenu.on('click', function (event) {
                    event.stopPropagation();
                    /* Act on the event */
                    var $this = $(this);

                    if ($this.hasClass('is-active')) {
                        $this.removeClass('is-active');
                    } else {
                        $navItem.parents('.header-main-mobile-nav-list').find('.header-main-mobile-nav-item').removeClass('is-active');
                        $this.find('.header-main-mobile-subnav-item').removeClass('is-active');
                        $this.addClass('is-active');
                        $this.slideDown();
                    }
                });
            }).catch(noop);
        },
        toggleSubNav: function toggleSubNav() {
            exist('.main-subnav-js').then(function (res) {
                var $tabSubnav = $(res);
                var $subNavItem = $('.header-main-mobile-subnav-item');

                $tabSubnav.on('click', function (event) {
                    event.preventDefault();
                    event.stopPropagation();
                    /* Act on the event */
                    var $this = $(this);
                    if ($this.hasClass('is-active')) {
                        $this.removeClass('is-active');
                    } else {
                        $subNavItem.parents('.header-main-mobile-subnav-list').find('.header-main-mobile-subnav-item').removeClass('is-active');
                        $this.addClass('is-active');
                        $this.slideDown();
                    }
                });
            }).catch(noop);
        },
        toggleChildNav: function toggleChildNav() {
            exist('.header-main-mobile-childnav-item').then(function (res) {
                var $tabSubnav = $(res);

                $tabSubnav.on('click', function (event) {
                    event.stopPropagation();
                    /* Act on the event */
                });
            }).catch(noop);
        }
    };

    var datePicker = {
        datePicker: function datePicker() {
            exist('.date-picker-js').then(function (res) {
                var $datePicker = $(res);
                var datepicker = $datePicker.flatpickr({
                    dateFormat: "d-m-Y",
                    onReady: function onReady(dateObj, dateStr, instance) {
                        $('.calendar-box').each(function (i, o) {
                            var $this = $(this);
                            if ($this.find('.calendar-clear').length < 1) {
                                $this.append('<a href="" class="calendar-clear">&times;</a>');
                            }
                        });
                    }
                });

                $('.calendar-box').on('click', '.calendar-clear', function (event) {
                    event.preventDefault();
                    /* Act on the event */
                    $(this).parent().find('.date-picker-js').val('');
                });
            }).catch(noop);
        },
        reportSearchDatePicker: function reportSearchDatePicker() {
            exist('.report-search-datepicker-js').then(function (res) {
                var $datePicker = $(res);
                var datepicker = $datePicker.flatpickr({
                    dateFormat: "j-M-y",
                    onReady: function onReady(dateObj, dateStr, instance) {
                        $('.calendar-box').each(function (i, o) {
                            var $this = $(this);
                            if ($this.find('.calendar-clear').length < 1) {
                                $this.append('<a href="" class="calendar-clear">&times;</a>');
                            }
                        });
                    }
                });

                $('.calendar-box').on('click', '.calendar-clear', function (event) {
                    event.preventDefault();
                    /* Act on the event */
                    $(this).parent().find('.report-search-datepicker-js').val('');
                });
            }).catch(noop);
        }
    };

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
            }).catch(noop);
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
            }).catch(noop);
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
            }).catch(noop);
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
            }).catch(noop);
        }
    };

    var scrollToTop = {
        scrollToTop: function scrollToTop() {
            exist('.scroll-top-container').then(function (res) {
                var $scrollContainer = $(res);
                var $scrollTopBtn = $('.scroll-top-btn');

                $(window).on('scroll', function (event) {
                    event.preventDefault();
                    /* Act on the event */
                    // console.log($(window).scrollTop())
                    if ($(window).scrollTop() > 1000) {
                        $scrollContainer.addClass('is-active');
                    } else if ($(window).scrollTop() < 1000) {
                        $scrollContainer.removeClass('is-active');
                    }
                });

                $scrollTopBtn.on('click', function (event) {
                    event.preventDefault();
                    /* Act on the event */
                    $('html, body').animate({
                        scrollTop: 0
                    }, 500);
                    // $(window).scrollTop(0);
                });
            }).catch(noop);
        }
    };

    var selectInput = function () {
        $('.select2-js').select2();
    };

    var clamp = function () {
        exist('.clamp-js').then(function (res) {
            var $clamp = $(res);

            $('.clamp-js').clamp({ clamp: 3 });
        }).catch(noop);
    };

    var calendarClose = {
        calendarClose: function calendarClose() {
            exist('.calendar-pop-up-js').then(function (res) {

                $(res).on('click', function (event) {
                    event.preventDefault();
                    event.stopPropagation();
                    /* Act on the event */
                    $('.calendar-pop-up').removeClass('is-active');
                    $('.pop-up-background').removeClass('is-active');
                    $('body').removeClass('overflow-hidden');
                });
            }).catch(noop);
        }
    };

    var livetime = function () {
        exist('.livetime-js').then(function (res) {
            var $time = $(res);

            function updateTime() {
                var date = moment(new Date());
                var time = moment(date).format('dddd, DD MMM YYYY | HH:mm');
                if ($(window).width() > 400) {
                    $time.html(time + ' WIB');
                } else {
                    $time.html(time);
                }
            }

            updateTime();
            setInterval(function () {
                updateTime();
            }, 1000);
        }).catch(noop);
    };

    var countdown = function () {
        // _.exist('.countdown-js').then(res => {
        //   	const $countdown = $('.countdown-js')
        //       const data = $countdown.attr('data-countdown');
        //       const targetDate = new Date(data)
        //       const currentDate = new Date()
        //       const convertedDate = (targetDate.getTime()/1000) - (currentDate.getTime()/1000);

        //       var clock = $('.countdown-js').FlipClock(targetDate, {
        // 	clockFace: 'DailyCounter',
        //           countdown: true
        // });

        // }).catch(_.noop)
    };

    var dropdownToggle = {
        searchOptionsToggle: function searchOptionsToggle() {
            exist('.report-search').then(function (res) {
                var $searchField = $(res);

                $searchField.on('click', '.toggle-options', function (e) {
                    e.preventDefault();
                    var $this = $(e.currentTarget);
                    var $searchOptions = $('.report-search-options');
                    var $relativeToggle = $('.report-search .relative-toggle');

                    $searchOptions.toggleClass('is-active');
                    setTimeout(function () {
                        return $relativeToggle.toggleClass('is-active');
                    }, 50);
                });
            }).catch(noop);
        },
        downloadMenuDropdown: function downloadMenuDropdown() {
            exist('.js-statistic-accordion').then(function (res) {
                var $toggleButton = $(res);

                $toggleButton.on('click', '.download-toggle', function (e) {
                    e.stopPropagation();
                    var $this = $(e.currentTarget);
                    var targetId = $this.data('toggle');
                    var $thisTarget = $(targetId);

                    $('.download-list').not(targetId).removeClass('is-active');
                    $thisTarget.toggleClass('is-active');
                }).on('click', '.download-list', function (e) {
                    e.stopPropagation();
                });

                $(document).on('click', function (e) {
                    $('.download-list').removeClass('is-active');
                });
            }).catch(noop);
        },
        downloadDropdown: function downloadDropdown() {
            exist('.download-toggle').then(function (res) {
                var $toggleButton = $(res);

                $toggleButton.on('click', function (e) {
                    e.stopPropagation();
                    var $this = $(e.currentTarget);
                    var targetId = $this.data('toggle');
                    var $thisTarget = $(targetId);

                    $('.download-list').not(targetId).removeClass('is-active');
                    $thisTarget.toggleClass('is-active');
                }).on('click', '.download-list', function (e) {
                    e.stopPropagation();
                });

                $(document).on('click', function (e) {
                    $('.download-list').removeClass('is-active');
                });
            }).catch(noop);
        },
        multiSelectDropdown: function multiSelectDropdown() {
            exist('.form-dropdown').then(function (res) {
                var $toggle = $(res);

                $toggle.on('click', function (e) {
                    e.stopPropagation();
                    var $this = $(e.currentTarget);
                    var targetId = $this.data('toggle');
                    var $thisTarget = $(targetId);

                    $('.form-dropdown__list').not(targetId).removeClass('is-active');
                    $thisTarget.toggleClass('is-active');
                });

                $('.form-dropdown__list').on('click', '.select-all', function (e) {

                    var $this = $(e.currentTarget);
                    var $parent = $this.parents('.form-dropdown__list');
                    var value = $this.is(':checked') ? $this.val() : 'None';

                    $parent.find('input[type="checkbox"]').prop('checked', $this.is(':checked'));
                    $parent.siblings('.form-dropdown').val(value);
                }).on('click', 'input[type="checkbox"]:not(.select-all)', function (e) {

                    var $this = $(e.currentTarget);
                    var $parent = $this.parents('.form-dropdown__list');
                    var $siblings = $parent.find('input[type="checkbox"]').not('.select-all');
                    var $checkedSiblings = $parent.find('input[type="checkbox"]:checked').not('.select-all');
                    var value = $siblings.length === $checkedSiblings.length ? $($parent.find('.select-all')[0]).val() : $checkedSiblings.length > 1 ? $checkedSiblings.length + ' selected' : $checkedSiblings.length === 1 ? $($checkedSiblings[0]).val() : 'None';

                    $parent.find('.select-all').prop('checked', $siblings.length === $checkedSiblings.length);
                    $parent.siblings('.form-dropdown').val(value);
                }).on('click', function (e) {
                    e.stopPropagation();
                });

                $(document).on('click', function (e) {
                    $('.form-dropdown__list').removeClass('is-active');
                });
            }).catch(noop);
        },
        gotoPageDropdown: function gotoPageDropdown() {
            exist('.js-select-url').then(function (res) {
                var $select = $(res);

                $select.on('change', function (e) {
                    var $this = $(e.currentTarget),
                        targetUrl = $('option:selected', $this).data('url');

                    window.location.href = './' + targetUrl;
                });
            }).catch(noop);
        }
    };

    var tooltip = {
        cellTooltip: function cellTooltip() {
            exist('.js-table-tooltip').then(function (res) {
                var $table = $(res);
                var $tooltip = $('#tooltiptext');

                $table.on('mousemove', '.tooltip', function (e) {
                    var text = $(e.currentTarget).find('.tooltip-data').html();
                    $tooltip.html(text).animate({ left: e.pageX + 10, top: e.pageY }, 0);
                    if (!$tooltip.is(':visible')) $tooltip.show();
                });

                $table.on('mouseleave', '.tooltip', function (e) {
                    $tooltip.hide();
                });
            }).catch(noop);
        }
    };

    var _extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; };
    var numFormat = {
    thousandSeparator: function thousandSeparator() {
        exist('.thousand-separator').then(function (res) {
            var $items = $(res);
            $.each($items, function (index) {
                var content = Number($items[index].innerHTML).toLocaleString('en');
                $items[index].innerHTML = content;
            });
        }).catch(noop);
    }
};

    var App = _extends({
        activeStateMobile: activeStateMobile,
        WPViewportFix: WPViewportFix,
        objectFitPolyfill: objectFitPolyfill
    }, tabMenu, navMenu, datePicker, accordion, filterDisplay, scrollToTop, {
            selectInput: selectInput,
            clamp: clamp
        }, calendarClose, {
            livetime: livetime,
            countdown: countdown
        }, dropdownToggle, tooltip, numFormat);

    for (var fn in App) {
        App[fn]();
    }

    return App;

}());
