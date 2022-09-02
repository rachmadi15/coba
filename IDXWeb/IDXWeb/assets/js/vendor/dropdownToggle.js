import * as _ from './utils.js'

export default {
    searchOptionsToggle() {
        _.exist('.report-search').then(res => {
            let $searchField = $(res)

            $searchField.on('click', '.toggle-options', e => {
                e.preventDefault()
                let $this = $(e.currentTarget)
                let $searchOptions = $('.report-search-options')
                let $relativeToggle = $('.report-search .relative-toggle')

                $searchOptions.toggleClass('is-active')
                setTimeout(() => $relativeToggle.toggleClass('is-active'), 50)
            })
        }).catch(_.noop)
    },

	downloadDropdown() {
		_.exist('.download-toggle').then(res => {
			const $toggleButton = $(res)

            $toggleButton.on('click', e => {
                e.stopPropagation()
                let $this = $(e.currentTarget)
                let targetId = $this.data('toggle')
                let $thisTarget = $(targetId)

                $('.download-list').not(targetId).removeClass('is-active')
                $thisTarget.toggleClass('is-active')
            }).on('click', '.download-list', e => {
                e.stopPropagation()
            })

            $(document).on('click', e => {
                $('.download-list').removeClass('is-active')
            })
		}).catch(_.noop)
	},

    multiSelectDropdown() {
        _.exist('.form-dropdown').then(res => {
            const $toggle = $(res)

            $toggle.on('click', e => {
                e.stopPropagation()
                let $this = $(e.currentTarget)
                let targetId = $this.data('toggle')
                let $thisTarget = $(targetId)

                $('.form-dropdown__list').not(targetId).removeClass('is-active')
                $thisTarget.toggleClass('is-active')
            })

            $('.form-dropdown__list').on('click', '.select-all', e => {

                let $this = $(e.currentTarget)
                let $parent = $this.parents('.form-dropdown__list')
                let value = $this.is(':checked') ? $this.val() : 'None'

                $parent.find('input[type="checkbox"]').prop('checked', $this.is(':checked'))
                $parent.siblings('.form-dropdown').val(value)

            }).on('click', 'input[type="checkbox"]:not(.select-all)', e => {

                let $this = $(e.currentTarget)
                let $parent = $this.parents('.form-dropdown__list')
                let $siblings = $parent.find('input[type="checkbox"]').not('.select-all')
                let $checkedSiblings = $parent.find('input[type="checkbox"]:checked').not('.select-all')
                let value =
                    ($siblings.length === $checkedSiblings.length)
                    ? $($parent.find('.select-all')[0]).val()
                    : ($checkedSiblings.length > 1)
                    ? $checkedSiblings.length+' selected'
                    : ($checkedSiblings.length === 1)
                    ? $($checkedSiblings[0]).val()
                    : 'None'

                $parent.find('.select-all').prop('checked', ($siblings.length === $checkedSiblings.length))
                $parent.siblings('.form-dropdown').val(value)

            }).on('click', e => {
                e.stopPropagation()
            })

            $(document).on('click', e => {
                $('.form-dropdown__list').removeClass('is-active')
            })
        }).catch(_.noop)
    },

    gotoPageDropdown() {
        _.exist('.js-select-url').then(res => {
            let $select = $(res)

            $select.on('change', e => {
                let $this = $(e.currentTarget),
                    targetUrl = $('option:selected', $this).data('url')

                window.location.href = `./${targetUrl}`
            })
        }).catch(_.noop)
    }
}
