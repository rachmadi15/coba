import * as _ from './utils.js'

export default function () {
    _.exist('.pop-up-background').then(res => {
    	var $container = $(res)

		$('body').on('click', '.pop-up-background', function(event) {
			event.preventDefault();
			/* Act on the event */
			$(this).removeClass('is-active')
			$(this).children().removeClass('is-active')
			$('body').removeClass('overflow-hidden')
		});

	}).catch(_.noop)
}
