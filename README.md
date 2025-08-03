### Design Considerations

- I'm trying to find a balance between code that is supposed to be high-performance and low latency, and not
  over-engineering the exercise.
    - I mean, resiliency, logging, telemetry, observability, DI patterns, where should I draw the line? :)
- This exercise sent me into a nice rabbit hole on how to properly secure strings in .NET.
  I already knew the SecureString class was not considered secure anymore,
  since any attacker would have access to the memory anyway.
  Still, this kind of application should strive to make the sensitive data as short-lived as possible,
  not only because of possible security breaches, but to avoid it appearing in any log, trace or memory dump.
  This is hard to do with the way strings are managed by the runtime.
  The alternative would be to force a collection everytime a credit card number went out of scope, which isn't feasible.
  My conclusion was that the bytes of the request Stream representing the sensitive data should be read directly into a
  pinned buffer,
  (like a Span\<T>), which could then be manually zeroed. I started to try and implement it, but realized it would be
  over-engineering for this exercise.
- I'm not really familiar with the Domain language of payment gateways, so I struggled a bit to name the entities.
  I also checked your API reference docs for inspiration, but the real world is way more complicated.
  I didn't know if what I received was a payment (submission, request, order) Same with what I sent to the bank. No big
  deal though, just registering my thoughts.
- The model is pretty simple, and given the performance requirements, I've opted for writing my own validation logic,
  since most of the ComponentModel validation attributes depend on reflection. I've limited my use of them where
  necessary for the Swagger documentation.
    - For object mapping, however, there is Mapperly, which is a source generating adapters, 