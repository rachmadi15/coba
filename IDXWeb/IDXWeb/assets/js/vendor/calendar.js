import * as _ from './utils.js'

export default {
	stockCalendar() {
		_.exist('.stock-calendar-js').then(res => {
			const $calendar = $(res)
			const dataCalendar = $calendar.attr('data-calendar');
			jQuery.getJSON(dataCalendar, function(response, textStatus) {
			  //optional stuff to do after success
				 $calendar.fullCalendar({
					events: response.dataEvent,
					eventLimit: true,
					fixedWeekCount : true,
					contentHeight: 355,
					eventColor: '#9F0E0F',
					eventLimitText: 'Event',
					locale: 'en',
				    views: {
				        agenda: {
				            eventLimit: 1
				        }
				    },
				    header: {
				    	left:   '',
					    center: 'prev,title,next',
					    right:  ''
				    },
				    eventRender: function(event, element) {
				        element.click( function() {
				        	$('.calendar-pop-up-date').html(moment(event.start).format('DD MMMM YYYY'))
				        	$('.calendar-pop-up-title').html(event.title)
				        	$('.calendar-pop-up-description').html(event.description)
				        	$('.calendar-pop-up-location').html(event.location)
				        	$('.calendar-pop-up').addClass('is-active');
				        	$('.calendar-background-js').addClass('is-active')
				        	$('body').addClass('overflow-hidden')
				        })
				    }
				})
			});

			$('.calendar-pop-up-js').on('click', function(event) {
				event.preventDefault();
				event.stopPropagation();
				/* Act on the event */
				$('.calendar-pop-up').removeClass('is-active')
				$('.pop-up-background').removeClass('is-active')
				$('body').removeClass('overflow-hidden')
			});
		}).catch(_.noop)
	},

	listedCompanyCalendar() {
		_.exist('.fullpage-calendar-js').then(res => {
			const $calendar = $(res)
			const dataCalendar = $calendar.attr('data-calendar');
			jQuery.getJSON(dataCalendar, function(response, textStatus) {
			  //optional stuff to do after success
				 $calendar.fullCalendar({
					events: response.dataEvent,
					eventLimit: true,
					fixedWeekCount : false,
					eventColor: '#9F0E0F',
					eventLimitText: 'Event',
				    views: {
				        agenda: {
				            eventLimit: 0
				        },
				        month: {
					    	columnFormat: 'dddd'
					    }
				    },				 
				    header: {
				    	left:   '',
					    center: 'prev,title,next',
					    right:  ''
				    },
				    eventRender: function(event, element) {
				        element.click( function() {
							$('.calendar-pop-up-date').html(moment(event.start).format('DD MMMM YYYY'))
				        	$('.calendar-pop-up-title').html(event.title)
				        	$('.calendar-pop-up-description').html(event.description)
				        	$('.calendar-pop-up-location').html(event.location)
				        	$('.calendar-pop-up').addClass('is-active');
				        	$('.pop-up-background').addClass('is-active')
				        	$('body').addClass('overflow-hidden')
				        })
				    }
				})
			});

			$('.calendar-pop-up-js').on('click', function(event) {
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
