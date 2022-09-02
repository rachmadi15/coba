import * as _ from './utils.js'

export default {
	cellTooltip() {
		_.exist('.js-table-tooltip').then(res => {
			const $table = $(res)
            let $tooltip = $('#tooltiptext')

            $table.on('mousemove', '.tooltip', function(e) {
                let text = $(e.currentTarget).find('.tooltip-data').html()
                $tooltip.html(text).animate({ left: e.pageX + 10, top: e.pageY }, 0)
                if (!$tooltip.is(':visible')) $tooltip.show()
            })

            $table.on('mouseleave', '.tooltip', function(e) {
                $tooltip.hide()
            })
		}).catch(_.noop)
	},
}
