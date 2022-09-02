import * as _ from './utils.js'

export default function () {
    _.exist('.clamp-js').then(res => {
    	var $clamp = $(res)

		$('.clamp-js').clamp({clamp:3});

	}).catch(_.noop)
}
