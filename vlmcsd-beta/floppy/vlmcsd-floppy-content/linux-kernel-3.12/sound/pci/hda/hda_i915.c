/*
 *  hda_i915.c - routines for Haswell HDA controller power well support
 *
 *  This program is free software; you can redistribute it and/or modify it
 *  under the terms of the GNU General Public License as published by the Free
 *  Software Foundation; either version 2 of the License, or (at your option)
 *  any later version.
 *
 *  This program is distributed in the hope that it will be useful, but
 *  WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY
 *  or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU General Public License
 *  for more details.
 *
 *  You should have received a copy of the GNU General Public License
 *  along with this program; if not, write to the Free Software Foundation,
 *  Inc., 59 Temple Place - Suite 330, Boston, MA  02111-1307, USA.
 */

#include <linux/init.h>
#include <linux/module.h>
#include <sound/core.h>
#include <drm/i915_powerwell.h>
#include "hda_i915.h"

static int (*get_power)(void);
static int (*put_power)(void);
static int (*get_cdclk)(void);

int hda_display_power(bool enable)
{
	if (!get_power || !put_power)
		return -ENODEV;

	snd_printdd("HDA display power %s \n",
			enable ? "Enable" : "Disable");
	if (enable)
		return get_power();
	else
		return put_power();
}

int haswell_get_cdclk(void)
{
	if (!get_cdclk)
		return -EINVAL;
	return get_cdclk();
}

int hda_i915_init(void)
{
	int err = 0;

	get_power = symbol_request(i915_request_power_well);
	if (!get_power) {
		snd_printk(KERN_WARNING "hda-i915: get_power symbol get fail\n");
		return -ENODEV;
	}

	put_power = symbol_request(i915_release_power_well);
	if (!put_power) {
		symbol_put(i915_request_power_well);
		get_power = NULL;
		return -ENODEV;
	}

	get_cdclk = symbol_request(i915_get_cdclk_freq);
	if (!get_cdclk)	/* may have abnormal BCLK and audio playback rate */
		snd_printd("hda-i915: get_cdclk symbol get fail\n");

	snd_printd("HDA driver get symbol successfully from i915 module\n");

	return err;
}

int hda_i915_exit(void)
{
	if (get_power) {
		symbol_put(i915_request_power_well);
		get_power = NULL;
	}
	if (put_power) {
		symbol_put(i915_release_power_well);
		put_power = NULL;
	}
	if (get_cdclk) {
		symbol_put(i915_get_cdclk_freq);
		get_cdclk = NULL;
	}

	return 0;
}
