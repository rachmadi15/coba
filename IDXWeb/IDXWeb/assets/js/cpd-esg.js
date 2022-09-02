
function getESG(){
            var kodeEmiten = getParams('kodeEmiten');
            var selectedYear = $('#hYearFilter option:selected').val();
			$.get('/umbraco/Surface/ListedCompany/GetESGInformation?kodeEmiten=' + kodeEmiten + '&year=' + selectedYear, function (result) {
				if(result.data.length > 0){
					$('#esgTime').text(moment(result.data[0].TanggalData).locale(currentLang).format('DD-MMM-YYYY'));
					$('#direksiWoman').text(result.data[0].RasioDireksiWanita);
					$('#direksiMan').text(result.data[0].RasioDireksiPria);
					$('#seniorWoman').text(result.data[0].RasioSeniorWanita);
					$('#seniorMan').text(result.data[0].RasioSeniorPria);
					$('#esgAvgTime').text(result.data[0].AvgWaktuPelatihan);
					$('#esgEmisi').text(result.data[0].PenggunaanEmisi);
					$('#esgDaurUlang').text(result.data[0].DaurUlangSampah);
				}else{
					$('#esgTime').text('-');
					$('#direksiWoman').text('-');
					$('#direksiMan').text('-');
					$('#seniorWoman').text('-');
					$('#seniorMan').text('-');
					$('#esgAvgTime').text('-');
					$('#esgEmisi').text('-');
					$('#esgDaurUlang').text('-');
				}
            });
		}