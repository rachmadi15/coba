import * as _ from './utils.js'

export default {
	scrollToTop() {
		_.exist('.scroll-top-container').then(res => {
			const $scrollContainer = $(res)
			const $scrollTopBtn = $('.scroll-top-btn');

			$(window).on('scroll', function(event) {
				event.preventDefault();
				/* Act on the event */
				// console.log($(window).scrollTop())
				if ( $(window).scrollTop() > 1000) {
					$scrollContainer.addClass('is-active')
				} else if ($(window).scrollTop() < 1000) {
					$scrollContainer.removeClass('is-active')
				}
			});

			$scrollTopBtn.on('click', function(event) {
				event.preventDefault();
				/* Act on the event */
				$('html, body').animate({
	                scrollTop: 0
	            }, 500)
				// $(window).scrollTop(0);
			});
									
		}).catch(_.noop)
	}
}
