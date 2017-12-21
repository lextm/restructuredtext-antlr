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
  :  comment | listItemBullet | listItemEnumerated | paragraph | lineBlock
  ;

comment
  :  Space* Comment Space* (lineNoBreak line*)?
  ;

paragraph
  :  line+
  ;

section
  :  (LineBreak Section)? title LineBreak Section (LineBreak)* sectionElement*
  ;

title
  : LineBreak textStart
  | LineBreak lineSpecial Space+ (paragraphNoBreak)?
  | line
  ;

lineBlock
  :  lineBlockAtom+
  ;

lineBlockAtom
  :  LineBreak Block Space indentation? lineAtom+
  ;

listItemBullet
  :  LineBreak Space* bullet Space* (paragraph+)?
  |  LineBreak Space* special=(Minus | Plus)
  ;

bullet
  :  Star 
  |  Minus 
  |  Plus
  ;

listItemEnumerated
  :  LineBreak enumerated=lineSpecial Space+ (paragraphNoBreak paragraph*)?
  ;
  
paragraphNoBreak
  :  lineNoBreak line*
  ;

lineNoBreak
  :  lineAtom+
  ;
  
line
  :  (LineBreak indentation?)? lineAtom+
  |  lineSpecial
  ;
  
lineAtom
  :  span
  |  text
  ;
  
text
  :  textStart textEnd?
  |  Dot
  |  '"' textEnd?
  |  Any textEnd?
  |  '(' textEnd?
  |  ':' textEnd?
  |  '}' textEnd?
  |  ')' textEnd?
  |  '\\' textEnd?
  |  Space textEnd
  ;

lineSpecial
  :  Numbers Dot
  //|  Alphabet Dot
  ;
  
empty_line
  :  LineBreak Space*
  ;

indentation
  :  Space+
  ;

textStart
  :  forcedText (text_fragment forcedText*)*
  |  text_fragment_firstTwo text_fragment*
  |  '/'
  |  '?'
  |  Equal Equal
  |  Alphabet text_fragment*
  ;

forcedText 
  :  '(*)' 
  |  '[*]' 
  |  '\'*\'' 
  |  '\'"*"\''
  ; 

textEnd
  :  textAtoms
  ;
  
textAtoms
  : text_fragment+
  ;

span
  :  starText
  |  reference
  |  referenceIn
  |  hyperlinkTarget
  |  hyperlink
  |  hyperlinkDoc
  |  backTickText
  |  quotedLiteral
  ;

quotedLiteral
  : '>' Space lineNoBreak
  ;

text_fragment_firstTwo
  :  (Star ~(Space | LineBreak))
  |  (Minus ~(Space | LineBreak))
  |  (Plus ~Space)
  |  (Numbers Dot ~(Space | LineBreak))
  |  (Block ~Space)
  |  ('\\' '"' Star '"' ~'\\')
  |  text_fragment_start text_fragment_start text_fragment
  ;
  
text_fragment_start
  :  Section
  |  SemiColon
  |  EscapeSequence
  |  UnderScore
  |  Numbers
  |  Alphabet
  |  TimeStar
  |  '/'
  |  '#'
  |  '['
  |  ']'
  |  '('
  |  ')'
  |  Colon
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
  |  Block
  |  Literal
  |  Comment
  |  Space
  |  Dot
  |  Minus
  |  Quote
  |  (Star Space)
  ;

starText
  :  Star Star* starAtoms Star+
  |  Star starNoSpace starAtoms LineBreak? Space* starNoSpace starAtoms Star*
  |  Star Star
  ;

starAtoms
  :  starAtom+ (Star* starAtom)*
  ;

starNoSpace
  :  ~(Star | LineBreak | Space)
  ;

starAtom
  :  ~(Star | LineBreak)
  ;

backTickText
  :  (Colon titled=Alphabet Colon)? body
  ;

body
//  :  (BackTick backTickText BackTick)
//  |  (BackTick backTickAtoms BackTick+)
  :  BackTick BackTick* backTickAtoms BackTick+
//  |  BackTick backTickNoSpace backTickAtoms LineBreak? Space* backTickNoSpace backTickAtoms BackTick*
  |  BackTick backTickNoSpace backTickAtoms BackTick*
  |  BackTick BackTick
  ;

backTickAtoms
  :  backTickAtom+
  ;

backTickNoSpace
  :  ~(BackTick | LineBreak | Space)
  ;

backTickAtom
  :  ~BackTick
  |  BackTick ~BackTick
  ;

reference
  :  Any+ UnderScore
  ;

referenceIn
  :  UnderScore hyperlinkAtom+ Colon Space url
  ;

hyperlinkTarget
  :  UnderScore Any+
  ;
  
hyperlink
  :  BackTick hyperlinkAtom+ Space '<' url '>' BackTick UnderScore
  ;
 
hyperlinkDoc
  :  ':doc:' BackTick hyperlinkAtom+ Space '<' url '>' BackTick
  |  ':doc:' BackTick url BackTick
  ;

url
  :  urlAtom+
  ;
  
urlAtom
  :  ~( LineBreak | BackTick )
  ;
  
hyperlinkAtom
  :  ~( LineBreak | '<' | '>' | BackTick | Star )
  ;

Literal
  :  Colon LineBreak LineBreak* Colon Colon
  ;

Section
  :  (Minus | Equal | Plus) (Minus | Equal | Plus) (Minus | Equal | Plus)+
  ;
  
TimeStar
  : Numbers Star
  | 'x' Star
  ;

Alphabet
  : [A-Za-z]+
  ;
  
Numbers
  : [0-9]+
  ;

Quote
  :  Colon Colon
  ;

Dot
  :  '.'
  ;
  
SemiColon
  :  ';'
  ;
  
Colon
  :  ':'
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
  :  '|'
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
  :  '\\' ('\\' | Star)
  ;

LineBreak
  :  '\r'? '\n'
  ;

Any
  :  .
  ;