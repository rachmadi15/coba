import * as _ from './utils.js'

export default {
	mainSlider() {
		_.exist('.main-slider').then(res => {
			const $slider = $(res)
			$slider.slick({
				autoplay: true,
				autoplaySpeed: 2000,
				arrows: false
			})
		}).catch(_.noop)
	},

	holidaySlider() {
		_.exist('.holiday-slider').then(res => {
			const $slider = $(res)
			$slider.slick({
				autoplay: false,
				autoplaySpeed: 2000,
				nextArrow: $('.btn-next-icon'),
				prevArrow: $('.btn-prev-icon')
			})
		}).catch(_.noop)
	}
}
