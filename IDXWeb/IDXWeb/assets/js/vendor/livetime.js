import * as _ from './utils.js'

export default function () {
    _.exist('.livetime-js').then(res => {
    	var $time = $(res)
		
		function updateTime(){
			var date = moment(new Date())
		    var time = moment(date).format('dddd, DD MMM YYYY | HH:mm');
		    if ( $(window).width() > 400) {      
				$time.html(time + ' WIB');
			} 
			else {
				$time.html(time);
			}
		};

		updateTime();
		setInterval(function(){
		   updateTime();
		},1000);

	}).catch(_.noop)
}
