grammar ReStructuredText;

// Copyright (C) 2011 Bart Kiers
// Copyright (C) 2017 Lex Li
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this
// software and associated documentation files (the "Software"), to deal in the Software
// without restriction, including without limitation the rights to use, copy, modify, merge,
// publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons
// to whom the Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or
// substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR
// PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE
// FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
// OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
// DEALINGS IN THE SOFTWARE.

options {
  language=CSharp;
}

parse
  :  (element | empty_line) + EOF
  ;

element
  :  section | sectionElement
  ;
  
sectionElement
  :  comment | listItemBullet | paragraph | listItemEnumerated | lineBlock
  ;

comment
  :  Space* Comment (line+)?
  ;

paragraph
  :  line+
  ;
  
section
  :  (Section LineBreak)? title Section (LineBreak)+ sectionElement*
  ;
  
lineBlock
  :  (Block line)+
  ;  

line
  :  indentation? text
  ;

empty_line
  :  Space* LineBreak
  ;

indentation
  :  Space+
  ;

text
  : text_fragment_start text_fragment* LineBreak
  ;
  
title
  : text_fragment+ LineBreak
  ;
 
text_fragment_start
  :  styledText
  |  interpretedText
  |  inlineLiteral
  |  reference
  |  referenceIn
  |  hyperlinkTarget
  |  hyperlink
  |  hyperlinkToc
  |  hyperlinkDoc
  |  url
  |  Section
  |  Star
  |  Plus
  |  Minus
  |  Colon
  |  EscapeSequence
  |  UnderScore
  |  DIGITS
  |  STRING
  |  '/'
  |  '.'
  |  '#'
  |  '['
  |  ']'
  |  '('
  |  ')'
  |  ';'
  |  Equal
  |  '?'
  |  '<'
  |  '>'
  |  '&'
  |  '"'
  |  Any
  ;

text_fragment
  :  text_fragment_start
  |  Space
  |  Block
  |  Bullet
  |  Literal
  |  Enumerated
  ;

styledText
  :  bold
  |  italic
  ;

bold
  :  Star Star boldAtom+ Star Star
  ;  

italic
  :  Star italicAtom+ (Star | StarSpace)
  ;

boldAtom
  :  ~Star | ~LineBreak
  |  italic
  ;

italicAtom
  :  ~Star | ~LineBreak
  |  bold
  ;

interpretedText
  :  BackTick interpretedTextAtoms BackTick
  ;

interpretedTextAtoms
  :  ~(BackTick | '<' | '>')+
  ;

inlineLiteral
  :  BackTick BackTick inlineLiteralAtoms BackTick BackTick
  ;

inlineLiteralAtoms
  :  inlineLiteralAtom+
  ;

inlineLiteralAtom
  :  ~BackTick
  |  BackTick ~BackTick
  ;

reference
  :  Any+ UnderScore
  ;

referenceIn
  :  UnderScore hyperlinkAtom+ ':' Space url
  ;

hyperlinkTarget
  :  UnderScore Any+
  ;
  
hyperlink
  :  BackTick hyperlinkAtom+ Space '<' url '>' BackTick UnderScore
  ;
  
hyperlinkToc
  :  hyperlinkAtom+ Space '<' url '>'
  ;
  
hyperlinkDoc
  :  ':doc:' BackTick hyperlinkAtom+ Space '<' url '>' BackTick
  |  ':doc:' BackTick url BackTick
  ;
  
hyperlinkAtom
  :  ~( LineBreak | '<' | '>' | '`' | '*' )
  ;

listItemBullet
  :  Space* Bullet (paragraph+)?
  ;

listItemEnumerated
  :  Enumerated paragraph+ ending=LineBreak? 
  ;
  
url
   : relativeUri
   | absoluteUri
   | 'mailto:' string '@' string
   ;

relativeUri
   : '/'? path '/' path
   ;

absoluteUri
   : scheme '://' login? host (':' port)? ('/' path )? '/'? query? frag?
   ;

scheme
   : string
   ;

host
   : '/'? (hostname | hostnumber)
   ;

hostname
   : string ('.' string)*
   ;

hostnumber
   : DIGITS '.' DIGITS '.' DIGITS '.' DIGITS
   ;

port
   : DIGITS
   ;

path
   : string ('/' string)*
   ;

user
   : string
   ;

login
   : user ':' password '@'
   ;

password
   : string
   ;

frag
   : ('#' string)
   ;

query
   : ('?' search)
   ;

search
   : searchparameter ('&' searchparameter)*
   ;

searchparameter
   : string (Equal (string | DIGITS | HEX))
   ;

string
   : '_'? STRING | DIGITS | EscapeSequence
   ;

DIGITS
   : [0-9] +
   ;


HEX
   : ('%' [a-fA-F0-9] [a-fA-F0-9]) +
   ;


STRING
   : ([a-zA-Z~0-9] | HEX | '(' | ')') ([a-zA-Z0-9.-] | HEX | '(' | ')' | '=' | '_' | '+')*
   ;


Colon
  :  ':'
  ;


Literal
  :  Colon LineBreak LineBreak* Colon Colon
  ;

Section
  :  ('-' | '=' | '+') ('-' | '=' | '+') ('-' | '=' | '+')+
  ;
  
Bullet
  :  StarSpace 
  |  Plus Space 
  |  Minus Space
  ;
  
Enumerated
  :  (DIGITS '.' Space)
  ;

StarSpace
  :  Star Space
  ;

Equal
  :  '='
  ;

Plus
  :  '+'
  ;

Minus
  :  '-'
  ;

Block
  :  '| '
  ;

Comment
  :  ('.. ' LineBreak?)
  |  ('..' LineBreak)
  ;

UnderScore
  :  '_'
  ;

BackTick
  :  '`'
  ;

Star
  :  '*'
  ;

Space
  :  ' ' 
  |  '\t'
  ;

EscapeSequence
  :  '\\' ('\\' | '*')
  ;

LineBreak
  :  '\r'? '\n'
  ;

Any
  :  .
  ;