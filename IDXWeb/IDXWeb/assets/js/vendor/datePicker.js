import * as _ from './utils.js'

export default {
	datePicker() {
		_.exist('.date-picker-js').then(res => {
			const $datePicker = $(res)
			var datepicker = $datePicker.flatpickr({
				dateFormat: "d-m-Y",
				onReady: function(dateObj, dateStr, instance) {
			        $('.calendar-box').each(function(i, o) {
			            var $this = $(this);
			            if ($this.find('.calendar-clear').length < 1) {
			                $this.append('<a href="" class="calendar-clear">&times;</a>');

			            }
			        });
			    }
			});

			$('.calendar-box').on('click', '.calendar-clear', function(event) {
				event.preventDefault();
				/* Act on the event */
				$(this).parent().find('.date-picker-js').val('')
			});
		}).catch(_.noop)
	},

    reportSearchDatePicker() {
        _.exist('.report-search-datepicker-js').then(res => {
            const $datePicker = $(res)
            var datepicker = $datePicker.flatpickr({
                dateFormat: "j-M-y",
                onReady: function(dateObj, dateStr, instance) {
                    $('.calendar-box').each(function(i, o) {
                        var $this = $(this);
                        if ($this.find('.calendar-clear').length < 1) {
                            $this.append('<a href="" class="calendar-clear">&times;</a>');

                        }
                    });
                }
            });

            $('.calendar-box').on('click', '.calendar-clear', function(event) {
                event.preventDefault();
                /* Act on the event */
                $(this).parent().find('.report-search-datepicker-js').val('')
            });
        }).catch(_.noop)
    }
}
