import * as _ from './utils.js'

export default {
	otcTrade() {
		_.exist('.otc-radio-js').then(res => {
			const $radio = $(res)
			const $otcContainer = $('.otc-filter-container')
			
			$radio.on('change', function(event) {
				event.preventDefault();
				/* Act on the event */
				$otcContainer.removeClass('is-active')
				const filterElem = $(this).val()
				$(`#`+filterElem).addClass('is-active')
			});
			
		}).catch(_.noop)
	}
}
