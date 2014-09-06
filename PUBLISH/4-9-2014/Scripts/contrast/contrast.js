/**
* To check iOS devices and versions
*/
function ozyGetOsVersion() {
	"use strict";
    var agent = window.navigator.userAgent.toLowerCase(),
        start = agent.indexOf( 'os ' );

    if ( /iphone|ipod|ipad/.test( agent ) && start > -1 ) {
        return window.Number( agent.substr( start + 3, 3 ).replace( '_', '.' ) );
    } else {
        return 0;
    };

};


jQuery(document).ready(function($) {
	"use strict";

	var sidr_side = 'right', ozyMainWindowWidth, subMenuWidth, subMenuOffset, newSubMenuPosition, ozyClickDrag, ozyIosVersion, ozyCurrentVideoContainer, ozyCurrentVideoContainerShield;
			/* 2/3/4th level menu  offscreen fix */
			ozyMainWindowWidth = $(window).width();
				
	/**
	* Sidr (side menu) init
	*/	
	ozyIosVersion = parseInt(ozyGetOsVersion());	
	
	jQuery('#sidr-menu').sidr( { 
		side: sidr_side,
		name: 'sidr',
		speed: 400
	} );

	$(window).resize(function() {
		$.sidr('close', 'sidr'); // Close
    });
	
	$(document).on("click", function(e) {
		if(parseInt(ozyIosVersion) === 0 || 
		parseInt(ozyIosVersion) >= 7 ) {
			var sidr_div = $("#sidr");
			if (!sidr_div.is(e.target) && !sidr_div.has(e.target).length) {
				$.sidr('close', 'sidr'); // Close
			}
		}
	});
	
	/* on mobile devices */
	$(document).on("touchstart", function(e) {
		var sidr_div = $("#sidr");
		if (!sidr_div.is(e.target) && !sidr_div.has(e.target).length) {
			$.sidr('close', 'sidr'); // Close
		}		
	});

	/* this block help to sidr work as expected on iOS devices. */
    if (parseInt(ozyIosVersion) > 0) {
		jQuery('#sidr-menu').click(function(e){
			if($(this).data('opened') == '1') {
				if(parseInt(ozyIosVersion) < 7) { //ios 6 need special process, since header and footer position as fixed
					$('#header,#footer').css('left', '0px');
				}
				$.sidr('close', 'sidr'); // Close
				$(this).data('opened', '0');
			}else{
				if(parseInt(ozyIosVersion) < 7) { //ios 6 need special process, since header and footer position as fixed
					$('#header,#footer').css('left', '-260px');
				}
				$.sidr('open', 'sidr'); // Open
				$(this).data('opened', '1');
			}
			e.preventDefault();
		});
	}
	
	
	/**
	* Back to top button
	*/
	$(window).scroll(function() {
		if($(this).scrollTop() >= 100) {
			$('#to-top-button').fadeIn('fast');	
		} else {
			$('#to-top-button').fadeOut('fast');
		}
	});

	$('#to-top-button').click(function(e) {
		e.preventDefault();
		$('body,html').animate({scrollTop:0},800);
	});
	
	/**
	* Footer language switcher
	*/	
	
	/**
	* Sidr (side menu) 'Custom Menu' widget handler, turns it into an accordion menu
	*/
//	$('#sidr .menu li a').click(function (e) {
//		if($(this).parent('li').hasClass('menu-item-has-children')) {
//			e.preventDefault();
//		}
//		var ullist = $(this).parent().children('ul:first');
//		ullist.slideToggle();
//	}).click();
});
