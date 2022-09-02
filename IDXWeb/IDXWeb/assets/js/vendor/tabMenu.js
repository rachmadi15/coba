import * as _ from './utils.js'

export default {
	tabStockInfo() {
		_.exist('.tab-anchor-js').then(res => {
			const $tabAnchor = $(res)
			const $tableContent = $('.table-information-content')

			$tabAnchor.on('click', function(event) {
				event.preventDefault();
				event.stopPropagation();
				/* Act on the event */
				$tabAnchor.removeClass('is-active')
				$tableContent.removeClass('is-active')

				const otherElem = $(this).attr('href')
				$(otherElem).addClass('is-active')
				$(this).addClass('is-active')
			});
		}).catch(_.noop)
	},

	tabMarketOverview() {
		_.exist('.tab-market-js').then(res => {
			const $tabAnchor = $(res)
			const $chartContent = $('.market-overview-chart-stock')

			$tabAnchor.on('click', function(event) {
				event.preventDefault();
				event.stopPropagation();
				/* Act on the event */
				$chartContent.removeClass('is-active')
				$tabAnchor.removeClass('is-active')
				const otherElem = $(this).attr('href')
				$(otherElem).addClass('is-active')
				$(this).addClass('is-active')
			});
		}).catch(_.noop)
	},

	tabTop5() {
		_.exist('.top-5-title-js').then(res => {
			const $tabAnchor = $(res)
			const $tableContent = $('.top-5-content-js')

			$tabAnchor.on('click', function(event) {
				event.preventDefault();
				/* Act on the event */
				$tableContent.removeClass('is-active')
				$tabAnchor.removeClass('is-active')
				const otherElem = $(this).attr('href')
				$(otherElem).addClass('is-active')
				$(this).addClass('is-active')
			});
		}).catch(_.noop)
	},

	tabPeople() {
		_.exist('.people-tab-js').then(res => {
			const $tabPeople = $(res)
			const $peopleContent = $('.people-container')
			const moreText = $('[data-more]').attr('data-more')
			const closeText = $('[data-more]').attr('data-close')

			$tabPeople.on('click', function(event) {
				event.preventDefault();
				/* Act on the event */
				$peopleContent.removeClass('is-active')
				$tabPeople.removeClass('is-active')
				const otherElem = $(this).attr('href')
				$(otherElem).addClass('is-active')
				$(this).addClass('is-active')

				$('.hof-figcaption-description').each(function(index, el) {
					if ($(this).height() > 108) {
						$(this).addClass('hof-figcaption-description--limit')
						$(this).parent().append('<a href="" class="hof-more is-active">'+moreText+'</a><a href="" class="hof-close">'+closeText+'</a>')
					}
				});
			});
		}).catch(_.noop)
	},

	tabHistory() {
		_.exist('.history-tab-js').then(res => {
			const $tabHistory = $(res)
			const $historyContent = $('.history-container')

			$tabHistory.on('click', function(event) {
				event.preventDefault();
				/* Act on the event */
				$historyContent.removeClass('is-active')
				$tabHistory.removeClass('is-active')
				const otherElem = $(this).attr('href')
				$(otherElem).addClass('is-active')
				$(this).addClass('is-active')
			});
		}).catch(_.noop)
	},

	tabStatistic() {
		_.exist('.statistic-tab-js').then(res => {
			const $tabStatistic = $(res)
			const $statisticContent = $('.statistic-container')

			$tabStatistic.on('click', function(event) {
				event.preventDefault();
				/* Act on the event */
				$statisticContent.removeClass('is-active')
				$tabStatistic.removeClass('is-active')
				const otherElem = $(this).attr('href')
				$(otherElem).addClass('is-active')
				$(this).addClass('is-active')
			});
		}).catch(_.noop)
	},

	tabEtpTrading() {
		_.exist('.etp-trading-tab-js').then(res => {
			const $tabEtp = $(res)
			const $etpContent = $('.etp-trading-container')

			$tabEtp.on('click', function(event) {
				event.preventDefault();
				/* Act on the event */
				$etpContent.removeClass('is-active')
				$tabEtp.removeClass('is-active')
				const otherElem = $(this).attr('href')
				$(otherElem).addClass('is-active')
				$(this).addClass('is-active')
			});
		}).catch(_.noop)
	},

	exchangeTrade() {
		_.exist('.exchange-trade-tab-js').then(res => {
			const $tabExchange = $(res)
			const $exchangeContent = $('.exchange-trade-container')

			$tabExchange.on('click', function(event) {
				event.preventDefault();
				/* Act on the event */
				$exchangeContent.removeClass('is-active')
				$tabExchange.removeClass('is-active')
				const otherElem = $(this).attr('href')
				$(otherElem).addClass('is-active')
				$(this).addClass('is-active')
			});
		}).catch(_.noop)
	},

	tabTradingSummary() {
		_.exist('.trading-summary-title-js').then(res => {
			const $tabAnchor = $(res)
			const $tableContent = $('.trading-summary-content-js')

			$tabAnchor.on('click', function(event) {
				event.preventDefault();
				/* Act on the event */
				$tableContent.removeClass('is-active')
				$tabAnchor.removeClass('is-active')
				const otherElem = $(this).attr('href')
				$(otherElem).addClass('is-active')
				$(this).addClass('is-active')
			});
		}).catch(_.noop)
	},

	companyProfileTab() {
		_.exist('.company-profile-anchor-js').then(res => {
			const $tabAnchor = $(res)
			const $tableContent = $('.company-profile-content')

			$tabAnchor.on('click', function(event) {
				event.preventDefault();
				/* Act on the event */
				$tableContent.removeClass('is-active')
				$tabAnchor.removeClass('is-active')
				const otherElem = $(this).attr('href')
				$(otherElem).addClass('is-active')
				$(this).addClass('is-active')
			});
		}).catch(_.noop)
	},

	companyProfileDropdown() {
		_.exist('.financial-report-js').then(res => {
			const $dropdown = $(res)
			const $contentContainer = $('.financial-report-content')

			$dropdown.on('change', function(event) {
				event.preventDefault();
				/* Act on the event */
				$contentContainer.removeClass('is-active')
				const otherElem = $dropdown.val()
				$(otherElem).addClass('is-active')
			});
		}).catch(_.noop)
	},

	tabMonitoringEffects() {
		_.exist('.monitoring-effects-tab-js').then(res => {
			const $tabEtp = $(res)
			const $etpContent = $('.monitoring-effects-container')

			$tabEtp.on('click', function(event) {
				event.preventDefault();
				/* Act on the event */
				$etpContent.removeClass('is-active')
				$tabEtp.removeClass('is-active')
				const otherElem = $(this).attr('href')
				$(otherElem).addClass('is-active')
				$(this).addClass('is-active')
			});
		}).catch(_.noop)
	},
}
