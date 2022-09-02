import * as _ from './utils.js'

export default {
	calendarClose() {
		_.exist('.calendar-pop-up-js').then(res => {

			$(res).on('click', function(event) {
				event.preventDefault();
				event.stopPropagation();
				/* Act on the event */
				$('.calendar-pop-up').removeClass('is-active')
				$('.pop-up-background').removeClass('is-active')
				$('body').removeClass('overflow-hidden')
			});
		}).catch(_.noop)
	}
}
