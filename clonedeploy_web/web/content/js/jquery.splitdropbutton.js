//
// Author: Pierre Lebrun
// Email: anthonylebrun@gmail.com
// Company: SmashingBoxes (http://smashingboxes.com)
//

;(function($) {

  var SplitDropButton = {
    defaults: {
      toggleDivContent: undefined
    }
  };

  /********************/
  /* Plugin Interface */
  /********************/

  SplitDropButton.publicMethods = {

    initialize: function(options) {
      return this.each(function() {
        var $this = $(this);

        if ($this.data('splitdrop') == null) {          
          var splitdrop = $.extend({}, SplitDropButton.privateMethods);
          var settings = $.extend({}, SplitDropButton.defaults, options || {})
          splitdrop.initialize(this, settings);
          $this.data('splitdrop', splitdrop);
        }
      });
    }
  };

  /******************************/
  /* Private (instance) methods */
  /******************************/

  SplitDropButton.privateMethods = {

    initialize: function(el, settings) {
      this.$el = $(el);
         
      var $children = this.$el.children('a');
      var $widestLink = null;
         
      $children.each(function(index, link) {
        if ($widestLink == null) {
          $widestLink = $(link);
        } else if ($(link).outerWidth() > $widestLink.outerWidth()) {
          $widestLink = $(link);
        }
      });
      
      this.links = {
        primary: $children[0],
        secondary: $children.slice(1, $children.length)
      };
      
       if($widestLink != null) 
      {
        this.linkWidth = $widestLink.outerWidth() + 'px';
      }
      else
      {
        $('.split-btn').hide();
      }
      this.linkHeight = $(this.links.primary).outerHeight() + 'px'; 
      
      this.$toggleDiv = $('<div class="spb-toggle">').append(settings.toggleDivContent);
      this.$primaryDiv = $('<div class="spb-primary">');
      this.$secondaryDiv = $('<div class="spb-secondary">');
      
      this.appendHTML();
      this.toggleSecondaryLinks();
      this.closeOnOutsideClick();
    },
    
    appendHTML: function() {
      $(this.links.primary).add(this.links.secondary).each(function(index, link) {
        $(link).css({'display': 'block'});
      });
      
      this.$el.css({
        'position':     'relative',
        'width':        this.linkHeight,
        'margin-left':  this.linkWidth
      });
        
      this.$toggleDiv.appendTo(this.$el).css({
        'height': this.linkHeight,
        'width': this.linkHeight,
        'cursor': 'pointer',
        'box-sizing': 'border-box',
        '-moz-box-sizing': 'border-box'
      });
      
      this.$primaryDiv.appendTo(this.$el).append(this.links.primary).css({
        'position': 'absolute',
        'height': this.linkHeight,
        'width': this.linkWidth,
        'top': '0',
        'right': this.linkHeight,
        'box-sizing': 'border-box',
        '-moz-box-sizing': 'border-box',
        
      });
        
      this.$secondaryDiv.appendTo(this.$el).append(this.links.secondary).css({
        'position': 'absolute',
        'width': this.linkWidth,
        'top': this.linkHeight,
        'right': this.linkHeight,
        'box-sizing': 'border-box',
        '-moz-box-sizing': 'border-box',
      });
      
      this.$secondaryDiv.hide();
    },
      
    toggleSecondaryLinks: function() {
      this.$toggleDiv.click($.proxy(function() {
        if (this.$toggleDiv.hasClass("spb-active")) {
          this.close();
        } else {
          this.open();
        }
      }, this));
    },
    
    closeOnOutsideClick: function() {
      $(document).on('click', $.proxy(function(e) {
        var $clicked = $(e.target);
        if (!$clicked.closest(this.$el).length) {
          this.close();
        }
      }, this));
    },
    
    open: function() {
      this.$el.css({'z-index': '1000'});
      this.$toggleDiv.addClass("spb-active");
      this.$primaryDiv.addClass("spb-activeprimary").removeClass("spb-inactiveprimary");
      this.$secondaryDiv.show();
    },
    
    close: function() {
      this.$el.css({'z-index': '0'});
      this.$toggleDiv.removeClass("spb-active");
      this.$primaryDiv.addClass("spb-inactiveprimary").removeClass("spb-activeprimary");
      this.$secondaryDiv.hide();
    }
    
  };

  /*******************************/
  /* wrapping it all in a plugin */
  /*******************************/

  $.fn.splitdropbutton = function(method) {
    if (SplitDropButton.publicMethods[method]) {
      return SplitDropButton.publicMethods[method].apply(this, Array.prototype.slice.call(arguments, 1));
    } else if (typeof method === 'object' || !method) {
      return SplitDropButton.publicMethods.initialize.apply(this, arguments);
    } else {
      $.error('Method ' + method + ' does not exist on jQuery.splitdropbutton');
    }
  };

})($);
