﻿--TLWP-1220 - Fix spelling errors in qualifications

update qualification 
set   Title = 'BTEC Level 3 National Certificate in Enterprise and Entrepreneurship',
      ModifiedBy = 'System',
      ModifiedOn = GETUTCDATE()
where larid = '60174134'
and   Title = 'BTEC Level 3 National Certificatein Enterprise and Entrepreneurship'

update qualification 
set   Title = 'BTEC Level 3 National Extended Certificate in Business',
      ModifiedBy = 'System',
      ModifiedOn = GETUTCDATE()
where larid = '60171595'
and   Title = 'BTEC Level 3 National Extended Certificatein Business'

update qualification 
set   Title = 'BTEC Level 3 National Certificate in Business',
      ModifiedBy = 'System',
      ModifiedOn = GETUTCDATE()
where larid = '60171558'
and   Title = 'BTEC Level 3 National Certificatein Business'

--select * from qualification where larid in ('60174134','60171595','60171558')
