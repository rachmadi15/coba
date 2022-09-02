import * as _ from './utils.js'

export default {
	openMobileNav() {
		_.exist('.open-nav-js').then(res => {
			const $openNav = $(res)
			const $mobileNav = $('.header-main-mobile-nav')

			$openNav.on('click', function(event) {
				event.preventDefault()
				event.stopPropagation()
				/* Act on the event */
				$mobileNav.toggleClass('is-active');
				$(this).toggleClass('fa-times');
				$(this).toggleClass('fa-bars');
			});
		}).catch(_.noop)
	},

	toggleMainNav() {
		_.exist('.main-nav-js').then(res => {
			const $tabMenu = $(res)
			const $navItem = $('.header-main-mobile-nav-item')

			$tabMenu.on('click', function(event) {
				event.stopPropagation();
				/* Act on the event */
				const $this = $(this)

				if ($this.hasClass('is-active')) {
					$this.removeClass('is-active')
				} else {
					$navItem.parents('.header-main-mobile-nav-list')
						.find('.header-main-mobile-nav-item')
	                    .removeClass('is-active');
	                $this
	                	.find('.header-main-mobile-subnav-item')
	   					.removeClass('is-active');
	                $this.addClass('is-active')
	                $this.slideDown();
				}
			});
		}).catch(_.noop)
	},

	toggleSubNav() {
		_.exist('.main-subnav-js').then(res => {
			const $tabSubnav = $(res)
			const $subNavItem = $('.header-main-mobile-subnav-item')

			$tabSubnav.on('click', function(event) {
				event.preventDefault();
				event.stopPropagation();
				/* Act on the event */
				const $this = $(this)
				console.log('halo')
				if ( $this.hasClass('is-active') ) {
					$this.removeClass('is-active')
				} else {
					$subNavItem.parents('.header-main-mobile-subnav-list')
						.find('.header-main-mobile-subnav-item')
	                    .removeClass('is-active');
	                $this.addClass('is-active');
	                $this.slideDown();
				}
			});

		}).catch(_.noop)
	},

	toggleChildNav() {
		_.exist('.header-main-mobile-childnav-item').then(res => {
			const $tabSubnav = $(res)

			$tabSubnav.on('click', function(event) {
				event.stopPropagation();
				/* Act on the event */
			});

		}).catch(_.noop)
	}
}
