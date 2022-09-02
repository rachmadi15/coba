import * as _ from './utils.js'

export default {
	searchOptionsToggle() {
		_.exist('.report-search').then(res => {
			let $searchField = $(res)

            $searchField.on('click', '.toggle-options', e => {
                e.preventDefault()
                let $this = $(e.currentTarget)
                let $searchOptions = $('.report-search-options')
                let $downloadToggle = $('.download-toggle')

                $searchOptions.toggleClass('is-active')
                setTimeout(() => $downloadToggle.toggleClass('is-active'), 50);
            })
		}).catch(_.noop)
	}
}
