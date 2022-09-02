import * as _ from './utils.js'

export default {
	bodAccordion() {
		_.exist('.hof-accordion-js').then(res => {
			const $hofContainer = $(res)
			const $showMore = $('.hof-more');
			const $close = $('.hof-close');

			const moreText = $('[data-more]').attr('data-more')
			const closeText = $('[data-more]').attr('data-close')

			$('.hof-figcaption-description').each(function(index, el) {
				if ($(this).height() > 108) {
					$(this).addClass('hof-figcaption-description--limit')
					$(this).parent().append('<a href="" class="hof-more is-active">'+moreText+'</a><a href="" class="hof-close">'+closeText+'</a>')
				}
			});

			$('body').on('click', '.hof-more', function(event) {
				event.preventDefault();
				/* Act on the event */
				$(this).removeClass('is-active')
				$(this).parent().find('.hof-figcaption-description').addClass('is-active')
				$(this).parent().find('.hof-close').addClass('is-active');
				$(this).parents('.hof-figure').addClass('is-active');
				$(this).parents('.hof-accordion-js').addClass('main-content')
			});

			$('body').on('click', '.hof-close', function(event) {
				event.preventDefault();
				/* Act on the event */
				$(this).removeClass('is-active')
				$(this).parent().find('.hof-figcaption-description').removeClass('is-active')
				$(this).parents('.hof-figure').removeClass('is-active');
				$(this).parent().find('.hof-more').addClass('is-active');
				$(this).parents('.hof-accordion-js').removeClass('main-content')
			});

		}).catch(_.noop)
	},

	faqAccordion() {
		_.exist('.accordion-text').then(res => {
			const $faqContainer = $(res)

			$faqContainer.on('click', function(event) {
				event.preventDefault();
				/* Act on the event */
				$(this).toggleClass('is-active');
                $(this).next().toggleClass('is-active');
			});

		}).catch(_.noop)
	},

    statisticAccordion() {
        _.exist('.js-statistic-accordion').then(res => {
            let $accordion = $(res)

            $accordion.on('click', '.accordion-head', e => {
                let $this = $(e.currentTarget)
                let $thisTarget = $($this.data('collapse'))

                $thisTarget.siblings().removeClass('is-active')

                $thisTarget.toggleClass('is-active')
            })

        }).catch(_.noop)
    }
}
